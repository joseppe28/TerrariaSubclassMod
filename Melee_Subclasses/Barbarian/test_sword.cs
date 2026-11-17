using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Content.Melee_Subclasses.Barbarian
{
    public class TestSword : ModItem
    {
        public override string Texture =>
            "Eventide/Content/Melee_Subclasses/Barbarian/test_sword";

        /// <summary>
        /// Configures the item's default combat properties.
        /// </summary>
        /// <remarks>Parameters: none.</remarks>
        /// <returns>No return value.</returns>
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;

            Item.width = 40;
            Item.height = 40;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;

            Item.autoReuse = true;

        }
    }
}
