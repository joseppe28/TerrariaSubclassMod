using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Content.Player_Moded;

namespace Content.Melee_Subclasses.Tank.armor.TestSet;

[AutoloadEquip(EquipType.Head)]
public class Test_Helmet : ModItem
{
    public override string Texture =>
        $"Terraria/Images/Item_{ItemID.IronHelmet}";

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.DamageType = DamageClass.Melee;

        Item.value = Item.sellPrice(silver: 10);
        Item.rare = ItemRarityID.White;

        Item.defense = 5;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Melee) += 0.05f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<Test_Chestplate>()
            && legs.type == ModContent.ItemType<Test_Leggings>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "Barbarian mode active and +25% more melee damage";
        player.GetModPlayer<Moded_Player_Stats>().setSubclass(Subclasses.Barbarian);
        player.GetModPlayer<Moded_Player_Stats>().addSubclassStatValue(Subclasses.Barbarian, 0.25f);
    }
}