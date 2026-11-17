using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Content.Melee_Subclasses.Paladin
{
    public class TestStaff : ModItem
    {
        public override string Texture => "Eventide/Content/Sprites/Idea_Sprites/lightning_staff";

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
            Item.useStyle = ItemUseStyleID.Thrust;

            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;

            Item.autoReuse = true;
        }
    }
}
