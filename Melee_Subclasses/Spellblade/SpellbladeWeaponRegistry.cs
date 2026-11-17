using System.Collections.Generic;

namespace Content.Melee_Subclasses.Spellblade;

/// <summary>Defines the new Manacost and ManaAddedPerhit for all Melee weapons with ranged effects.</summary>
/// <param name="ManaCost">The Mana Cost for the ranged effect of the weapon not the melee attack.</param>
/// <param name="ManaAddedPerHit">The amount of Mana added per melee hit.</param>
public sealed record WeaponStats(
    int ManaCost = 0,
    int ManaAddedPerHit = 0
    );




public static class SpellbladeWeaponRegistry
{
    public static Dictionary<int, WeaponStats> Weapons { get; } = new();

    public static List<int> MeleeProjectiles { get; } = new();
    /// <summary>
    /// Adds a weapon to this subclass registry or replaces its existing statistics.
    /// </summary>
    /// <param name="itemType">The vanilla ItemID or registered ModContent item type.</param>
    /// <param name="stats">The subclass-specific statistics to associate with this weapon.</param>
    /// <returns>No return value.</returns>
    public static void Register(int itemType, WeaponStats stats)
    {
        Weapons[itemType] = stats;
    }

    /// <summary>
    /// Adds a projectile to this subclass registry or replaces its existing statistics.
    /// </summary>
    /// <param name="projectileType">The vanilla ProjectileID or registered ModContent projectile type.</param>
    /// <param name="stats">The subclass-specific statistics to associate with this projectile.</param>
    /// <returns>No return value.</returns>
    public static void RegisterProjectile(int projectileType)
    {
        MeleeProjectiles.Add(projectileType);
    }
}
