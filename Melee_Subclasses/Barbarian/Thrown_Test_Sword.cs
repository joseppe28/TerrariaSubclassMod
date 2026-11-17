using Terraria;
using Terraria.ModLoader;

namespace Content.Melee_Subclasses.Barbarian
{
    public class ThrownTestSword : ModProjectile
    {
        // Reuse your sword's texture.
        public override string Texture =>
            "Eventide/Content/Melee_Subclasses/Barbarian/test_sword";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Add some rotation to the thrown sword
            Projectile.rotation += 0.3f * Projectile.direction;
            Projectile.velocity.Y += 0.15f; // Gravity
        }
    }
}