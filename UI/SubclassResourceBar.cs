using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Content.Player_Moded;
using Content.Melee_Subclasses.Barbarian;
using Content.Melee_Subclasses.Paladin;
using System;

namespace Content.UI
{
    public class SubclassResourceBar : UIState
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            Player player = Main.LocalPlayer;
            

            var stats = player.GetModPlayer<Moded_Player_Stats>();

            float current;
            float max;
            Color color;

            switch (stats.Subclass)
            {
                case Subclasses.Barbarian:
                    current = stats.CurrentRage;
                    max = 100f;
                    color = Color.Red;
                    break;

                case Subclasses.Paladin when player.HeldItem != null && PaladinWeaponRegistry.Weapons.ContainsKey(player.HeldItem.type):
                    current = PaladinGlobalItem.GetBlockHealth(player.HeldItem);
                    max = PaladinWeaponRegistry.Weapons[player.HeldItem.type].MaxBlockHealth;
                    color = Color.CornflowerBlue;
                    break;

                default:
                    return;
            }

            float percentage = MathHelper.Clamp(
                current / Math.Max(1f, max),
                0f,
                1f
            );

            // Position on screen
            int x = Main.screenWidth / 2 - 100;
            int y = Main.screenHeight - 100;

            // Background
            Rectangle background = new Rectangle(x, y, 200, 20);

            // Filled portion
            Rectangle fill = new Rectangle(
                x + 2,
                y + 2,
                (int)(196 * percentage),
                16
            );

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                background,
                Color.Black
            );

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                fill,
                color
            );
        }
    }
}