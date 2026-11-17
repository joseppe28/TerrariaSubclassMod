using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Content.Player_Moded;

namespace Content.Melee_Subclasses.Barbarian;

public sealed class BarbarianGlobalItem : GlobalItem
{
    public override bool InstancePerEntity => true;
    private bool originalNoMelee;
    private bool originalNoUseGraphic;
    private int originalShoot;
    private bool throwing;

    /// <summary>
    /// Stores the original item flags and adds the registered throw projectile when the item has none.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <returns>No return value.</returns>
    public override void SetDefaults(Item item)
    {
        originalNoMelee = item.noMelee;
        originalNoUseGraphic = item.noUseGraphic;
        originalShoot = item.shoot;
        var config = BarbarianWeaponRegistry.Weapons[item.type];
        if (config.ThrowProjectile > 0 && item.shoot == ProjectileID.None)
        {
            item.shoot = config.ThrowProjectile;
            item.shootSpeed = config.ThrowSpeed;
        }
    }

    /// <summary>
    /// Prepares the weapon graphic and contact-damage flags for a normal attack or alternate throw.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>True when normal item use is allowed; otherwise false.</returns>
    public override bool CanUseItem(Item item, Player player)
    {
        throwing = CanThrow(item, player) && player.altFunctionUse == 2;
        item.noMelee = throwing || originalNoMelee;
        item.noUseGraphic = throwing || originalNoUseGraphic;
        return true;
    }

    /// <summary>
    /// Restores the original item flags after a throw finishes or the player switches weapons.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>No return value.</returns>
    public override void UpdateInventory(Item item, Player player)
    {
        if (throwing && (player.HeldItem != item || player.itemAnimation == 0))
        {
            throwing = false;
            item.noMelee = originalNoMelee;
            item.noUseGraphic = originalNoUseGraphic;
        }
    }

    /// <summary>
    /// Attaches this GlobalItem only to weapons registered for this subclass.
    /// </summary>
    /// <param name="entity">The item being checked for a registry entry.</param>
    /// <param name="lateInstantiation">Whether tModLoader is attaching this global after initial item setup.</param>
    /// <returns>True when the item type is registered for this subclass; otherwise false.</returns>
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return BarbarianWeaponRegistry.Weapons.ContainsKey(entity.type);
    }

    /// <summary>
    /// Checks whether the player is a Barbarian and this weapon has a configured throw projectile.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>True when this weapon can perform an alternate throw; otherwise false.</returns>
    private static bool CanThrow(Item item, Player player)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        return stats.Subclass == Subclasses.Barbarian
            && BarbarianWeaponRegistry.Weapons[item.type].ThrowProjectile > 0;
    }

    /// <summary>
    /// Enables alternate use when this weapon can perform a Barbarian throw.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <returns>True when a Barbarian throw is available; otherwise false.</returns>
    public override bool AltFunctionUse(Item item, Player player)
    {
        return CanThrow(item, player);
    }

    /// <summary>
    /// Prevents contact damage during an alternate throw while preserving normal hit decisions.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="target">The NPC affected by the attack.</param>
    /// <returns>False during an alternate throw; null to preserve the normal hit decision.</returns>
    public override bool? CanHitNPC(Item item, Player player, NPC target)
    {
        if (CanThrow(item, player) && player.altFunctionUse == 2)
        {
            return false;
        }

        return null;
    }

    /// <summary>
    /// Uses the registered projectile and speed for an alternate Barbarian throw.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="position">The projectile spawn position; left unchanged.</param>
    /// <param name="velocity">The projectile velocity, updated for a configured throw.</param>
    /// <param name="type">The projectile type, updated for a configured throw.</param>
    /// <param name="damage">The projectile damage; left unchanged.</param>
    /// <param name="knockback">The projectile knockback; left unchanged.</param>
    /// <returns>No return value.</returns>
    public override void ModifyShootStats(Item item, Player player, ref Vector2 position,
        ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (!CanThrow(item, player) || player.altFunctionUse != 2)
        {
            return;
        }

        var stats = BarbarianWeaponRegistry.Weapons[item.type];
        type = stats.ThrowProjectile;
        velocity = velocity.SafeNormalize(Vector2.UnitX * player.direction) * stats.ThrowSpeed;
    }

    /// <summary>
    /// Allows original projectile attacks and configured alternate throws.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="source">The item-use source for the projectile.</param>
    /// <param name="position">The projectile spawn position; left unchanged.</param>
    /// <param name="velocity">The projectile velocity, updated for a configured throw.</param>
    /// <param name="type">The projectile type, updated for a configured throw.</param>
    /// <param name="damage">The projectile damage.</param>
    /// <param name="knockback">The projectile knockback; left unchanged.</param>
    /// <returns>True to allow a projectile to spawn; otherwise false.</returns>
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        return originalShoot > 0 || (CanThrow(item, player) && player.altFunctionUse == 2);
    }

    /// <summary>
    /// Adds the weapon's configured rage and equipment bonus after a direct Barbarian hit.
    /// </summary>
    /// <param name="item">The weapon instance being processed.</param>
    /// <param name="player">The player holding or using the weapon.</param>
    /// <param name="target">The NPC affected by the attack.</param>
    /// <param name="hit">The resolved hit information.</param>
    /// <param name="damageDone">The actual damage dealt to the target.</param>
    /// <returns>No return value.</returns>
    public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();
        if (stats.Subclass == Subclasses.Barbarian && damageDone > 0)
        {
            float rageGained = BarbarianWeaponRegistry.Weapons[item.type].RageAddedOnHit + stats.RageAddedOnHitBonus;
            stats.addRage(rageGained);
        }
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
        if (stats.Subclass == Subclasses.Barbarian)
        {
            damage += stats.getSubclassStat(Subclasses.Barbarian)  + stats.CurrentRage / 100f;
        }
    }
}
