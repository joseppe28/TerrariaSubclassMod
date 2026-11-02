using Terraria.ModLoader;
using Content.Melee_Subclasses.Paladin;
using Content.Melee_Subclasses.Barbarian;
using Content.Melee_Subclasses.Spellblade;
using Terraria.ID;

namespace Content;

public sealed class WeaponRegistrySystem : ModSystem
{
    /// <summary>
    /// Registers subclass weapons after content IDs exist and before item defaults are initialized.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void ResizeArrays()
    {
        PaladinWeaponRegistry.Register(ModContent.ItemType<TestStaff>(), new());
        BarbarianWeaponRegistry.Register(ModContent.ItemType<TestSword>(),
            new(ThrowProjectile: ModContent.ProjectileType<ThrownTestSword>()));
        SpellbladeWeaponRegistry.Register(ItemID.EnchantedSword, new(100, 30));
        SpellbladeWeaponRegistry.Register(ItemID.TerraBlade, new(100, 10, true));
        SpellbladeWeaponRegistry.Register(ItemID.StarWrath, new(100, 10));
    }

    // For now all Vanilla weapons are registered without a throw, perhaps i will add it later on
    public void RegisterBarbarianVanillaWeapons()
    {

        // Vanilla Pre-Hardmode Shortswords
        BarbarianWeaponRegistry.Register(ItemID.CopperShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.TinShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.IronShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.LeadShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.SilverShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.TungstenShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.GoldShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.PlatinumShortsword , new(2,0));
        BarbarianWeaponRegistry.Register(ItemID.Ruler , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.Umbrella , new(4,0));
        BarbarianWeaponRegistry.Register(ItemID.TragicUmbrella , new(4,0));
        BarbarianWeaponRegistry.Register(ItemID.Gladius , new(7,0));


        // Vanilla Pre-Hardmode Broadswords
        BarbarianWeaponRegistry.Register(ItemID.CopperBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.TinBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.IronBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.LeadBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.SilverBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.TungstenBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.GoldBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.PlatinumBroadsword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BorealWoodSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.WoodenSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.PalmWoodSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.EbonwoodSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.ShadewoodSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.RichMahoganySword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.AshWoodSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.CactusSword , new(7,0));
        BarbarianWeaponRegistry.Register(ItemID.BladedGlove , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.ZombieArm , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.Flymeal , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BreathingReed , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BatBat , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.TentacleSpike , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.CandyCaneSword , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.Katana , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.DyeTradersScimitar , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BloodButcherer , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.PurpleClubberfish , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.FalconBlade , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.FieryGreatsword , new(5,0));


        //Hardmode Shortswords
        BarbarianWeaponRegistry.Register(ItemID.ChlorophyteSaber , new(15,0));
        BarbarianWeaponRegistry.Register(ItemID.PiercingStarlight , new(10,0));

        //Hardmode Broadswords
        BarbarianWeaponRegistry.Register(ItemID.PearlwoodSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.TaxCollectorsStickOfDoom , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.SlapHand , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.CobaltSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.PalladiumSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.DD2SquireDemonSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.MythrilSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.OrichalcumSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.AdamantiteSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.TitaniumSword , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.BreakerBlade , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.Cutlass , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.FetidBaghnakhs , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.HamBat , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.Excalibur , new(15,0));
        BarbarianWeaponRegistry.Register(ItemID.TrueExcalibur , new(15,0));
        BarbarianWeaponRegistry.Register(ItemID.PsychoKnife , new(5,0));


        //Flails
        BarbarianWeaponRegistry.Register(ItemID.Mace , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.FlamingMace , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BallOHurt , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.TheMeatball , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.BlueMoon , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.Sunfury , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.ChainKnife , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.DripplerFlail , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.DaoofPow , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.FlowerPow , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.Flairon , new(10,0));
        // Just got added in 1.4.5 so need to wait
        //BarbarianWeaponRegistry.Register(ItemID.FlaironFlail , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.Anchor , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.ChainGuillotines , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.KOCannon , new(10,0));
        BarbarianWeaponRegistry.Register(ItemID.GolemFist , new(10,0));




        // Others
        BarbarianWeaponRegistry.Register(ItemID.Terragrim , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.Arkhalis , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.MonkStaffT1 , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.MonkStaffT3 , new(5,0));
        BarbarianWeaponRegistry.Register(ItemID.SolarEruption , new(5,0));

    }

    /// <summary>
    /// Clears weapon registrations when the mod unloads.
    /// </summary>
    /// <remarks>Parameters: none.</remarks>
    /// <returns>No return value.</returns>
    public override void Unload()
    {
        PaladinWeaponRegistry.Weapons.Clear();
        BarbarianWeaponRegistry.Weapons.Clear();
        SpellbladeWeaponRegistry.Weapons.Clear();
    }
}
