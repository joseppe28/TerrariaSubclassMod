using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Content.Melee_Subclasses.Tank.armor.TestSet;

[AutoloadEquip(EquipType.Body)]
public class Test_Chestplate : ModItem
{
    public override string Texture =>
        $"Terraria/Images/Item_{ItemID.IronChainmail}";

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;

        Item.value = Item.sellPrice(silver: 15);
        Item.rare = ItemRarityID.White;

        Item.defense = 10;
    }
}