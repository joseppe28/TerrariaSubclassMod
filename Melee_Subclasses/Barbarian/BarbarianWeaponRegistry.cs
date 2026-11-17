using System.Collections.Generic;

namespace Content.Melee_Subclasses.Barbarian;
/// <summary>Defines the rage and optional throw statistics of a registered weapon type.</summary>
/// <param name="RageAddedOnHit">The rage gained per direct hit before equipment bonuses.</param>
/// <param name="ThrowProjectile">The alternate throw projectile type, or zero to preserve ordinary alternate use.</param>
/// <param name="ThrowSpeed">The projectile speed of a configured alternate throw.</param>
public sealed record WeaponStats(
    float RageAddedOnHit = 10f,
    int ThrowProjectile = 0,
    float ThrowSpeed = 12f);

public static class BarbarianWeaponRegistry
{
    public static Dictionary<int, WeaponStats> Weapons { get; } = new();
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
}
