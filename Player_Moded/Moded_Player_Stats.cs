using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Content.Melee_Subclasses.Paladin;

namespace Content.Player_Moded;

public partial class Moded_Player_Stats : ModPlayer
{
    public PlayerState State { get; set; } = PlayerState.Idle;
    public Subclasses Subclass { get; set; } = Subclasses.None;
    private readonly Dictionary<Subclasses, float> subclassStats = new();
    public float MaxRage { get; set; }
    private float current_rage;
    public float CurrentRage => current_rage;
    public float RageAddedOnHitBonus { get; set; }
    public float RageLostOnHitPenaltyPercentage { get; set; } = 10f;
    public float RageLostPerSecondPercentage { get; set; } = 1f;
    public float blockRecoveryRate { get; set; } = 0f;
    public float BlockDamageReduction { get; set; } = 0f;
    public float BlockMovementMultiplier { get; set; } = 1f;
    private Item previousWeapon;
    private Subclasses previousSubclass;

    /// <summary>
    /// Adds rage and limits it to the player's current maximum.
    /// </summary>
    /// <param name="amount">The amount of rage to add or remove.</param>
    /// <returns>No return value.</returns>
    public void addRage(float amount)
    {
        current_rage = Math.Clamp(current_rage + amount, 0f, Math.Max(0f, MaxRage));
    }

    /// <summary>
    /// Reduces rage without allowing it to fall below zero.
    /// </summary>
    /// <param name="amount">The amount of rage to add or remove.</param>
    /// <returns>No return value.</returns>
    public void reduceRage(float amount)
    {
        current_rage = Math.Max(0f, current_rage - amount);
    }

    /// <summary>
    /// Resets equipment-derived subclass bonuses before equipment applies them again.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void ResetEffects()
    {
        Subclass = Subclasses.None;
        RageAddedOnHitBonus = 0f;
        MaxRage = 0f;
        blockRecoveryRate = 0f;
        BlockDamageReduction = 0f;
        BlockMovementMultiplier = 1f;
        if (State == PlayerState.Blocking)
        {
            State = PlayerState.Idle;
        }

        foreach (Subclasses key in subclassStats.Keys.ToArray())
        {
            subclassStats[key] = 0f;
        }
    }

    /// <summary>
    /// Resets state after weapon or subclass changes, updates the held weapon before applying its player modifiers, and limits current rage.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void PostUpdateEquips()
    {
        Item item = Player.HeldItem;
        bool changed = !ReferenceEquals(previousWeapon, item) || previousSubclass != Subclass;
        if (changed)
        {
            State = PlayerState.Idle;
        }

        previousWeapon = item;
        previousSubclass = Subclass;
        if (Player.dead || Subclass != Subclasses.Barbarian)
        {
            current_rage = 0f;
        }

        current_rage = Math.Min(current_rage, Math.Max(0f, MaxRage));

        if (item.TryGetGlobalItem<PaladinGlobalItem>(out PaladinGlobalItem weapon))
        {
            weapon.UpdateHeldWeapon(item, Player);
        }

        Player.moveSpeed *= BlockMovementMultiplier;
    }

    /// <summary>
    /// Applies the movement modifier supplied by the held weapon to the player's running speeds.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void PreUpdateMovement()
    {
        Player.maxRunSpeed *= BlockMovementMultiplier;
        Player.accRunSpeed *= BlockMovementMultiplier;
    }

    /// <summary>
    /// Applies passive rage decay each update.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void PostUpdate()
    {
        reduceRage(current_rage * RageLostPerSecondPercentage / 600f);
    }

    /// <summary>
    /// Forwards incoming hit modifiers to the held weapon's Paladin blocking mechanics.
    /// </summary>
    /// <param name="modifiers">The incoming hit modifiers before the damage is resolved.</param>
    /// <returns>No return value.</returns>
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Player.HeldItem.TryGetGlobalItem<PaladinGlobalItem>(out PaladinGlobalItem weapon))
        {
            weapon.ModifyPlayerHurt(Player.HeldItem, Player, ref modifiers);
        }
    }

    /// <summary>
    /// Removes a portion of current rage and forwards the hit to the held weapon's Paladin mechanics.
    /// </summary>
    /// <param name="info">The received hit, including the actual damage taken.</param>
    /// <returns>No return value.</returns>
    public override void OnHurt(Player.HurtInfo info)
    {
        reduceRage(current_rage * RageLostOnHitPenaltyPercentage / 100f);
        if (Player.HeldItem.TryGetGlobalItem<PaladinGlobalItem>(out PaladinGlobalItem weapon))
        {
            weapon.HandlePlayerHurt(Player.HeldItem, Player, info);
        }
    }

    /// <summary>
    /// Clears temporary subclass state, rage and weapon modifiers when the player dies.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void UpdateDead()
    {
        State = PlayerState.Idle;
        current_rage = 0f;
        BlockDamageReduction = 0f;
        BlockMovementMultiplier = 1f;
    }

    /// <summary>
    /// Sets the equipment-derived damage bonus for a subclass.
    /// </summary>
    /// <param name="subclass">The subclass whose damage bonus is accessed.</param>
    /// <param name="stat">The damage bonus to assign.</param>
    /// <returns>No return value.</returns>
    public void setSubclassStat(Subclasses subclass, float stat)
    {
        subclassStats[subclass] = stat;
    }

    /// <summary>
    /// Adds to a subclass damage bonus, creating its entry when necessary.
    /// </summary>
    /// <param name="subclass">The subclass whose damage bonus is accessed.</param>
    /// <param name="value">The damage bonus to add.</param>
    /// <returns>No return value.</returns>
    public void addSubclassStatValue(Subclasses subclass, float value)
    {
        if (subclassStats.ContainsKey(subclass))
        {
            subclassStats[subclass] += value;
        }
        else
        {
            subclassStats[subclass] = value;
        }
    }

    /// <summary>
    /// Gets the equipment-derived damage bonus for a subclass.
    /// </summary>
    /// <param name="subclass">The subclass whose damage bonus is accessed.</param>
    /// <returns>The registered subclass damage bonus, or zero when no entry exists.</returns>
    public float getSubclassStat(Subclasses subclass)
    {
        if (subclassStats.TryGetValue(subclass, out float value))
        {
            return value;
        }

        return 0f;
    }
}
