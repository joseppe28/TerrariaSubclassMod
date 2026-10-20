using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Content.Player_Moded;

namespace Content.Melee_Subclasses.Tank
{
    public class TestSword : ModItem
    {
        public override string Texture =>
            "TestMod/Content/Melee_Subclasses/Tank/test:sword";

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

            // for throwing 
            Item.shoot = ModContent.ProjectileType<ThrownTestSword>();
            Item.shootSpeed = 12f;

            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;       // Disable the held sword's damage.
                Item.noUseGraphic = true; // Hide it while throwing.
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                // LEFT CLICK
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.damage = 25;
                Item.knockBack = 5f;
            }

            return base.CanUseItem(player);
        }

        public override void OnHitNPC(
            Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageDone > 0)
            {
                if(player.GetModPlayer<Moded_Player_Stats>().getSubclass() == Subclasses.Barbarian)
                {
                    player.GetModPlayer<Moded_Player_Stats>().addRage(1); // Increase rage by 1 on hit
                }
            }
        }

        public override bool Shoot(
            Player player,
            Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            return player.altFunctionUse == 2;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.GetModPlayer<Moded_Player_Stats>().getSubclass() == Subclasses.Barbarian)
            {
                damage += player.GetModPlayer<Moded_Player_Stats>().getSubclassStat(Subclasses.Barbarian);
            }
        }
    }
}