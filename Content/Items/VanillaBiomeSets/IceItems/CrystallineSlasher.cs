using LunarVeil.Content.Bases;
using LunarVeil.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace LunarVeil.Content.Items.VanillaBiomeSets.IceItems
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class CrystallineSlasher : ModItem
    {
        int AttackCounter;
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.LunarVeil.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<CrystallineSwordSlash>();
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //     Particle.NewParticle<SnowflakeParticle>(position, -velocity, Color.White);

            int dir = AttackCounter;
            AttackCounter = -AttackCounter;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1, dir);
            return false;
        }

    }

    public class CrystallineSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "LunarVeil/Content/Items/VanillaBiomeSets/IceItems/CrystallineSlasher";

        int slash1time = 0;
        public override void SetDefaults()
        {
            //Set swing variables here
            //How long the swing lasts in ticks
            swingTime = 120;

            //How far the trail extends upward (away the player)
            trailTopWidth = 10;

            //How far the trail extends downward (towards the player)
            trailBottomWidth = 304;

            //The number of points in the trail, higher means the trail is longer
            trailCount = 65;

            //The distance from the player where the trail starts
            distanceToOwner = 0;

            //Brightness/Transparency of trail, 255 is fully opaque
            alpha = 200;

            slash1time = SwingTime;

            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 100;
            Projectile.width = 100;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = Swing_Speed_Multiplier - 1;
            Projectile.usesLocalNPCImmunity = true;

            //Multiplying by the thing so it's still 10 ticks
            Projectile.localNPCHitCooldown = 10 * Swing_Speed_Multiplier;
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            TrailTexture = ModContent.Request<Texture2D>("LunarVeil/Assets/Textures/CrystallineTrailSlash");
            GradientTexture = ModContent.Request<Texture2D>("LunarVeil/Assets/Textures/CrystallineTrailGradient");
        }

        public override void Unload()
        {
            if (Main.dedServ)
                return;

            TrailTexture = null;
            GradientTexture = null;
        }

        protected override void SwingAI()
        {

            if(Projectile.timeLeft >= 96)
            {

                Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                float multiplier = 0.2f;
                RGB *= multiplier;

                Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                int dir = (int)Projectile.ai[1];
                float lerpValue = Utils.GetLerpValue(0f, SwingTime, slash1time, true);

                //Smooth it some more
                float swingProgress = Easing.InBack(lerpValue);

                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation

                //How wide is the swing, in radians
                float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4;
                float start = defRot - swingRange;

                // ending rotation
                float end = (defRot + swingRange);

                // current rotation obv
                // angle lerp causes some weird things here, so just use a normal lerp
                float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);

                // offsetted cuz sword sprite
                Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;

                Owner.heldProj = Projectile.whoAmI;
                Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                Owner.itemRotation = rotation * Owner.direction;
                Owner.itemTime = 2;
                Owner.itemAnimation = 2;
            }

            if (Projectile.timeLeft >= 72 && Projectile.timeLeft < 96)
            {

                Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                float multiplier = 0.2f;
                RGB *= multiplier;

                Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                int dir = (int)Projectile.ai[1];
                float lerpValue = Utils.GetLerpValue(0f, SwingTime, slash1time, true);

                //Smooth it some more
                float swingProgress = Easing.InOutExpo(lerpValue);

                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation

                //How wide is the swing, in radians
                float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4;
                float start = defRot + swingRange;

                // ending rotation
                float end = (defRot - swingRange);

                // current rotation obv
                // angle lerp causes some weird things here, so just use a normal lerp
                float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);

                // offsetted cuz sword sprite
                Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;

                Owner.heldProj = Projectile.whoAmI;
                Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                Owner.itemRotation = rotation * Owner.direction;
                Owner.itemTime = 2;
                Owner.itemAnimation = 2;
            }

            if (Projectile.timeLeft >= 48 && Projectile.timeLeft < 72)
            {

                Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                float multiplier = 0.2f;
                RGB *= multiplier;

                Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                int dir = (int)Projectile.ai[1];
                float lerpValue = Utils.GetLerpValue(0f, 48, 48, true);

                //Smooth it some more
                float swingProgress = Easing.SpikeInBack(lerpValue);

                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation

                //How wide is the swing, in radians
                float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.PiOver4;
                float start = defRot - swingRange;

                // ending rotation
                float end = (defRot + swingRange);

                // current rotation obv
                // angle lerp causes some weird things here, so just use a normal lerp
                float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);

                // offsetted cuz sword sprite
                Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;

                Owner.heldProj = Projectile.whoAmI;
                Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                Owner.itemRotation = rotation * Owner.direction;
                Owner.itemTime = 2;
                Owner.itemAnimation = 2;
            }



            if (Projectile.timeLeft < 48)
            {

                Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                float multiplier = 0.2f;
                RGB *= multiplier;

                Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                int dir = (int)Projectile.ai[1];
                float lerpValue = Utils.GetLerpValue(0f, 48, 48, true);

                //Smooth it some more
                float swingProgress = Easing.InBack(lerpValue);

                // the actual rotation it should have
                float defRot = Projectile.velocity.ToRotation();
                // starting rotation

                //How wide is the swing, in radians
                float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.PiOver4;
                float start = defRot + swingRange;

                // ending rotation
                float end = (defRot - swingRange);

                // current rotation obv
                // angle lerp causes some weird things here, so just use a normal lerp
                float rotation = dir == 1 ? MathHelper.Lerp(start, end, swingProgress) : MathHelper.Lerp(end, start, swingProgress);

                // offsetted cuz sword sprite
                Vector2 position = Owner.RotatedRelativePoint(Owner.MountedCenter);
                position += rotation.ToRotationVector2() * holdOffset;
                Projectile.Center = position;
                Projectile.rotation = (position - Owner.Center).ToRotation() + MathHelper.PiOver4;

                Owner.heldProj = Projectile.whoAmI;
                Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                Owner.itemRotation = rotation * Owner.direction;
                Owner.itemTime = 2;
                Owner.itemAnimation = 2;
            }

        }
    }
}