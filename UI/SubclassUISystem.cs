using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Content.UI
{
    [Autoload(Side = ModSide.Client)]
    public class SubclassUISystem : ModSystem
    {
        private UserInterface resourceInterface;
        private SubclassResourceBar resourceBar;

        public override void Load()
        {
            resourceBar = new SubclassResourceBar();
            resourceBar.Activate();

            resourceInterface = new UserInterface();
            resourceInterface.SetState(resourceBar);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            resourceInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(
            List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(
                layer => layer.Name == "Vanilla: Resource Bars"
            );

            if (index == -1)
                return;

            layers.Insert(index, new LegacyGameInterfaceLayer(
                "TestMod: Subclass Resource Bar",
                () =>
                {
                    resourceInterface?.Draw(
                        Main.spriteBatch,
                        new GameTime()
                    );

                    return true;
                },
                InterfaceScaleType.UI
            ));
        }
    }
}