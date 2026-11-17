using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Content.Player_Moded;

namespace Content.Melee_Subclasses.Paladin.accessories
{
    public class PaladinEmblem : ModItem
    {
        public override string Texture =>
            $"Terraria/Images/Item_{ItemID.CrossNecklace}";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Moded_Player_Stats stats = player.GetModPlayer<Moded_Player_Stats>();

            if (stats.Subclass == Subclasses.None)
            {
                stats.Subclass = Subclasses.Paladin;
            }
        }
    }
}
