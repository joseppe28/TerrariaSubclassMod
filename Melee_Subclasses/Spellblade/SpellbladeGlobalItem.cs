using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Content.Player_Moded;
using Microsoft.Xna.Framework;

namespace Content.Melee_Subclasses.Spellblade;

public sealed class SpellbladeGlobalItem : GlobalItem
{
    /// <summary>
    /// Attaches this GlobalItem only to weapons registered for this subclass.
    /// </summary>
    /// <param name="entity">The item being checked for a registry entry.</param>
    /// <param name="lateInstantiation">Whether tModLoader is attaching this global after initial item setup.</param>
    /// <returns>True when the item type is registered for this subclass; otherwise false.</returns>
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return SpellbladeWeaponRegistry.Weapons.ContainsKey(entity.type);
    }

    /// <summary>
    /// Adds the active subclass damage bonus to this registered weapon though only to the projectiles not the melee attacks.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="position">The position from which the projectile is fired.</param>
    /// <param name="velocity">The velocity of the projectile.</param>
    /// <param name="type">The type of the projectile.</param>
    /// <param name="damage">The damage modifier to update.</param>
    /// <returns>No return value.</returns>
    public override void ModifyShootStats(
        Item item,
        Player player,
        ref Vector2 position,
        ref Vector2 velocity,
        ref int type,
        ref int damage,
        ref float knockback)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.Subclass == Subclasses.Spellblade)
        {
            damage = (int)(damage * (1f + stats.getSubclassStat(Subclasses.Spellblade)));
        }
    }   


    /// <summary>
    /// Handles the shooting of a projectile by a spellblade.
    /// </summary>
    /// <param name="item">The weapon instance being used.</param>
    /// <param name="player">The player using the weapon.</param>
    /// <param name="source">The source of the shot.</param>
    /// <param name="position">The position from which the projectile is fired.</param>
    /// <param name="velocity">The velocity of the projectile.</param>
    /// <param name="type">The type of the projectile.</param>
    /// <param name="damage">The damage of the projectile.</param>
    /// <param name="knockback">The knockback of the projectile.</param>
    /// <returns>True if the projectile is successfully shot; otherwise false.</returns>
    public override bool Shoot(
        Item item,
        Player player,
        EntitySource_ItemUse_WithAmmo source,
        Vector2 position,
        Vector2 velocity,
        int type,
        int damage,
        float knockback)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        WeaponStats weaponStats = SpellbladeWeaponRegistry.Weapons[item.type];
        int manaCost = weaponStats.ManaCost;
        if (stats.Subclass == Subclasses.Spellblade)
        {
            if (!player.CheckMana(manaCost, true) && !SpellbladeWeaponRegistry.MeleeProjectiles.Contains(type))
            {
                return false;
            }
            else
            {
                player.manaRegenDelay = player.maxRegenDelay;
                return true;
            
            }
        }

        return false;
    }


    public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.Subclass == Subclasses.Spellblade && damageDone > 0)
        {
            WeaponStats weaponStats = SpellbladeWeaponRegistry.Weapons[item.type];
            int manaAdded = weaponStats.ManaAddedPerHit;
            player.statMana += manaAdded;
            player.ManaEffect(manaAdded);
        }
    }
    
}