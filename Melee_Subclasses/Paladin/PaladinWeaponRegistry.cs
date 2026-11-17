using System.Collections.Generic;

namespace Content.Melee_Subclasses.Paladin;

/// <summary>Defines the blocking statistics shared by all instances of a registered weapon type.</summary>
/// <param name="MaxBlockHealth">The maximum block durability.</param>
/// <param name="DamageReduction">The blocked damage fraction, from zero to one.</param>
/// <param name="MovementMultiplier">The movement fraction retained while blocking, from zero to one.</param>
/// <param name="RecoveryPerSecond">The block durability recovered per second before equipment bonuses.</param>
/// <param name="CanBlock">Whether this registered weapon allows blocking.</param>
public sealed record WeaponStats(
    float MaxBlockHealth = 100f,
    float DamageReduction = 0.1f,
    float MovementMultiplier = 0.05f,
    float RecoveryPerSecond = 5f,
    bool CanBlock = true);

public static class PaladinWeaponRegistry
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
