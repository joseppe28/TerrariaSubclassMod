using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Content.Player_Moded;

namespace Content.Melee_Subclasses.Paladin;

public sealed class PaladinGlobalItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    private float health = -1f;
    private int pendingBlockedDamage;
    public bool Broken { get; private set; }
    public float BlockHealth => health;

    private int BaseUseStyle = -1;

    /// <summary>
    /// Attaches this GlobalItem only to weapons registered for this subclass.
    /// </summary>
    /// <param name="entity">The item being checked for a registry entry.</param>
    /// <param name="lateInstantiation">Whether tModLoader is attaching this global after initial item setup.</param>
    /// <returns>True when the item type is registered for this subclass; otherwise false.</returns>
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return PaladinWeaponRegistry.Weapons.ContainsKey(entity.type);
    }

    public static float GetBlockHealth(Item item)
    {
        if (item.TryGetGlobalItem<PaladinGlobalItem>(out var globalItem))
        {
            if (globalItem.health < 0f)
            {
                return 0f;
            }
            return globalItem.BlockHealth;
        }
        return 0f;
    }

    /// <summary>
    /// Reads held right-click, updates blocking and recovery, and supplies player modifiers.
    /// Called by PostUpdateEquips after equipment resolves and before modifiers are applied.
    /// </summary>
    /// <param name="item">The registered Paladin weapon currently held by the player.</param>
    /// <param name="player">The player whose input and blocking state are updated.</param>
    /// <returns>No return value.</returns>
    internal void UpdateHeldWeapon(Item item, Player player)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        bool wantsBlock = player.controlUseTile;
        bool blocking = wantsBlock && CanBlock(item, player);

        if (blocking)
        {
            stats.State = PlayerState.Blocking;
            SetBlockModifiers(item, stats);
            SuppressItemControls(player);
        }
        else if (stats.State == PlayerState.Blocking)
        {
            stats.State = PlayerState.Idle;
        }

        Recover(item, player);
    }

    /// <summary>
    /// Copies the weapon's configured blocking effects into the player's temporary modifiers.
    /// </summary>
    /// <param name="item">The registered weapon supplying the blocking statistics.</param>
    /// <param name="stats">The player state that applies these modifiers to Terraria's player stats.</param>
    /// <returns>No return value.</returns>
    private void SetBlockModifiers(Item item, Moded_Player_Stats stats)
    {
        WeaponStats configuration = PaladinWeaponRegistry.Weapons[item.type];
        stats.BlockDamageReduction = Math.Clamp(configuration.DamageReduction, 0f, 1f);
        stats.BlockMovementMultiplier = Math.Clamp(configuration.MovementMultiplier, 0f, 1f);
    }

    /// <summary>
    /// Consumes item-use controls so blocking takes priority over attacks and tile interactions.
    /// </summary>
    /// <param name="player">The player whose normal item-use controls are suppressed this tick.</param>
    /// <returns>No return value.</returns>
    private void SuppressItemControls(Player player)
    {
        player.controlUseTile = false;
        player.controlUseItem = false;
        player.releaseUseTile = false;
    }

    /// <summary>
    /// Registers a final-damage callback so durability measures only damage prevented by blocking.
    /// Forwarded from ModPlayer.ModifyHurt before Terraria resolves the hit.
    /// </summary>
    /// <param name="item">The held registered Paladin weapon.</param>
    /// <param name="player">The player whose blocking state and reduction are checked.</param>
    /// <param name="modifiers">The incoming hit modifiers receiving the blocking callback.</param>
    /// <returns>No return value.</returns>
    internal void ModifyPlayerHurt(Item item, Player player, ref Player.HurtModifiers modifiers)
    {
        pendingBlockedDamage = 0;
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.State != PlayerState.Blocking || !CanBlock(item, player))
        {
            return;
        }

        float reduction = Math.Clamp(stats.BlockDamageReduction, 0f, 1f);
        modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) => ApplyBlockReduction(ref info, reduction);
    }

    /// <summary>
    /// Reduces damage after other defenses and records the amount this block prevented.
    /// Terraria's minimum of one damage is retained when calculating prevented damage.
    /// </summary>
    /// <param name="info">The resolved hit before the blocking reduction is applied.</param>
    /// <param name="reduction">The fraction of remaining incoming damage the weapon blocks.</param>
    /// <returns>No return value.</returns>
    private void ApplyBlockReduction(ref Player.HurtInfo info, float reduction)
    {
        int damageWithoutBlock = info.Damage;
        int damageWithBlock = Math.Max(1, (int)(damageWithoutBlock * (1f - reduction)));
        pendingBlockedDamage = Math.Max(0, damageWithoutBlock - damageWithBlock);
        info.Damage = damageWithBlock;
    }

    /// <summary>
    /// Consumes durability using the damage prevented by the resolved block and ends an exhausted block.
    /// Forwarded from ModPlayer.OnHurt because GlobalItem has no player-hurt hook.
    /// </summary>
    /// <param name="item">The held registered Paladin weapon receiving block damage.</param>
    /// <param name="player">The player whose blocking state is checked and updated.</param>
    /// <param name="info">The received hit, including the actual damage taken.</param>
    /// <returns>No return value.</returns>
    internal void HandlePlayerHurt(Item item, Player player, Player.HurtInfo info)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        int blockedDamage = pendingBlockedDamage;
        pendingBlockedDamage = 0;
        if (stats.State != PlayerState.Blocking || blockedDamage <= 0)
        {
            return;
        }

        AbsorbHit(item, blockedDamage);
        if (Broken)
        {
            stats.State = PlayerState.Idle;
            stats.BlockDamageReduction = 0f;
            stats.BlockMovementMultiplier = 1f;
        }
    }

    /// <summary>
    /// Gets the weapon's maximum block health, with a lower limit of zero.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <returns>The nonnegative maximum block health.</returns>
    private float GetMaximumBlockHealth(Item item)
    {
        return Math.Max(0f, PaladinWeaponRegistry.Weapons[item.type].MaxBlockHealth);
    }

    /// <summary>
    /// Initializes block health to its maximum the first time this item is accessed.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <returns>No return value.</returns>
    private void InitializeBlockHealth(Item item)
    {
        if (health < 0f)
        {
            health = GetMaximumBlockHealth(item);
        }
    }

    /// <summary>
    /// Checks whether the weapon and player currently allow blocking.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>True when the player can block with this weapon; otherwise false.</returns>
    private bool CanBlock(Item item, Player player)
    {
        InitializeBlockHealth(item);
        return PaladinWeaponRegistry.Weapons[item.type].CanBlock && !Broken && health > 0f
            && player.GetModPlayer<Moded_Player_Stats>().Subclass == Subclasses.Paladin
            && !player.dead && !player.CCed && !player.noItems;
    }

    /// <summary>
    /// Restores block health while the player holds the weapon without blocking. A broken block recovers fully before it becomes usable again.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>No return value.</returns>
    private void Recover(Item item, Player player)
    {
        InitializeBlockHealth(item);
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.Subclass != Subclasses.Paladin || stats.State == PlayerState.Blocking)
        {
            return;
        }

        float weaponRecovery = PaladinWeaponRegistry.Weapons[item.type].RecoveryPerSecond;
        float recoveryPerSecond = Math.Max(0f, weaponRecovery + stats.blockRecoveryRate);
        float recoveryPerTick = recoveryPerSecond / 60f;
        health = Math.Min(GetMaximumBlockHealth(item), health + recoveryPerTick);
        if (health >= GetMaximumBlockHealth(item))
        {
            Broken = false;
        }
    }

    /// <summary>
    /// Consumes block health using damage prevented by blocking and marks an exhausted block as broken.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="damage">The damage prevented by this weapon on the resolved hit.</param>
    /// <returns>No return value.</returns>
    private void AbsorbHit(Item item, int damage)
    {
        InitializeBlockHealth(item);
        health = Math.Max(0f, health - damage);
        if (health <= 0f)
        {
            Broken = true;
        }
    }
    

    /// <summary>
    /// Prevents normal weapon use while the player is blocking or starting a block.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>True when normal item use is allowed; otherwise false.</returns>
    public override bool CanUseItem(Item item, Player player)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.State == PlayerState.Blocking || (player.controlUseTile && CanBlock(item, player)))
        {
            BaseUseStyle = item.useStyle;
            item.useStyle = ItemUseStyleID.HoldUp;
            item.noUseGraphic = false;
            return true;
        }
        item.useStyle = BaseUseStyle >= 0 ? BaseUseStyle : item.useStyle;
        return true;
    }

    /// <summary>
    /// Adds the active subclass damage bonus to this registered weapon.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="damage">The damage modifier to update.</param>
    /// <returns>No return value.</returns>
    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.Subclass == Subclasses.Paladin)
        {
            damage += stats.getSubclassStat(Subclasses.Paladin);
        }
    }

    /// <summary>
    /// Saves this weapon instance's block health and broken status.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="tag">The item save data to write or read.</param>
    /// <returns>No return value.</returns>
    public override void SaveData(Item item, TagCompound tag)
    {
        InitializeBlockHealth(item);
        tag["blockHealth"] = health;
        tag["broken"] = Broken;
    }

    /// <summary>
    /// Restores saved block health and broken status, or initializes missing health to its maximum.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="tag">The item save data to write or read.</param>
    /// <returns>No return value.</returns>
    public override void LoadData(Item item, TagCompound tag)
    {
        if (tag.ContainsKey("blockHealth"))
        {
            health = Math.Clamp(tag.GetFloat("blockHealth"), 0f, GetMaximumBlockHealth(item));
        }
        else
        {
            health = GetMaximumBlockHealth(item);
        }

        Broken = tag.GetBool("broken") || health <= 0f;
    }
}
