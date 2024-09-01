using LunarVeil.Systems;
using LunarVeil.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace LunarVeil.Content.Bases
{
    public abstract class BaseSwingProjectile : ModProjectile
    {
        protected float[] oldRotate;
        protected float[] oldDistanceToOwner;
        protected float[] oldLength;

        public Vector2 Top;
        public Vector2 Bottom;


        public static Asset<Texture2D> TrailTexture;
        public static Asset<Texture2D> GradientTexture;
     

        protected bool init;

        //How long the swing lasts in ticks
        protected int swingTime;

        //How far the trail extends upward (away the player)
        protected int trailTopWidth = 10;

        //How far the trail extends downward (towards the player)
        protected int trailBottomWidth = 300;

        //The number of points in the trail
        protected int trailCount = 65;

        //The distance from the player where the trail starts
        public float distanceToOwner = 0;

        //Brightness/Transparency of trail, 255 is fully opaque
        public int alpha = 255;

        public Player Owner => Main.player[Projectile.owner];



        //This is for smoothin the trail
        public const int Swing_Speed_Multiplier = 8;

        protected int SwingTime => (int)((swingTime * Swing_Speed_Multiplier) / Owner.GetAttackSpeed(Projectile.DamageType));
        public float holdOffset = 60f;

        //Ending Swing Time so it doesn't immediately go away after the swing ends, makes it look cleaner I think
        public int EndSwingTime = 6 * Swing_Speed_Multiplier;

        public override void SetDefaults()
        {
            Projectile.timeLeft = SwingTime;
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

            TrailTexture = ModContent.Request<Texture2D>("LunarVeil/Assets/Textures/ExampleTrailSlash");
            GradientTexture = ModContent.Request<Texture2D>("LunarVeil/Assets/Textures/ExampleTrailGradient");
        }

        public override void Unload()
        {
            if (Main.dedServ)
                return;

            TrailTexture = null;
            GradientTexture = null;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            DrawSlashTrail();

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);


            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0); // drawing the sword itself

            return false;
        }



        protected virtual float ControlTrailBottomWidth(float factor)
        {
            return trailBottomWidth * (1 - factor);
        }

        protected virtual void GetCurrentTrailCount(out float count)
        {
            count = 0f;
            if (oldRotate == null)
                return;

            for (int i = 0; i < oldRotate.Length; i++)
                if (oldRotate[i] != 100f)
                    count += 1f;
        }

        public override bool ShouldUpdatePosition() => false;

        private void InitTrailing()
        {
            if (!init)
            {
                oldRotate = new float[trailCount];
                oldDistanceToOwner = new float[trailCount];
                oldLength = new float[trailCount];
                if (Main.myPlayer == Projectile.owner)
                    Owner.direction = Main.MouseWorld.X > Owner.Center.X ? 1 : -1;
                for (int j = trailCount - 1; j >= 0; j--)
                {
                    oldRotate[j] = 100f;
                    oldDistanceToOwner[j] = distanceToOwner;
                    oldLength[j] = Projectile.height * Projectile.scale;
                }
                init = true;
            }
        }
        private void UpdateTrailing()
        {
            Vector2 rotVector2 = Projectile.velocity;
            Top = Projectile.Center + rotVector2 * (Projectile.scale * Projectile.height / 2 + trailTopWidth);
            Bottom = Projectile.Center - rotVector2 * (Projectile.scale * Projectile.height / 2);
            for (int i = trailCount - 1; i > 0; i--)
            {
                oldRotate[i] = oldRotate[i - 1];
                oldDistanceToOwner[i] = oldDistanceToOwner[i - 1];
                oldLength[i] = oldLength[i - 1];
            }

            oldRotate[0] = Projectile.rotation - MathHelper.PiOver4;
            oldDistanceToOwner[0] = distanceToOwner;
            oldLength[0] = Projectile.height * Projectile.scale;
        }
        protected virtual void SwingAI()
        {
            Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
            float multiplier = 0.2f;
            RGB *= multiplier;

            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            int dir = (int)Projectile.ai[1];
            float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);

            //Smooth it some more
            float swingProgress = Easing.InOutExpo(lerpValue, 6f);

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
        public sealed override void AI()
        {
            base.AI();
            InitTrailing();
            SwingAI();
            //Update Trails
            UpdateTrailing();
        }

        protected virtual void DrawSlashTrail()
        {
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            List<VertexPositionColorTexture> bars = new();
            GetCurrentTrailCount(out float count);

            for (int i = 0; i < count; i++)
            {
                if (oldRotate[i] == 100f)
                    continue;

                float factor = 1f - i / count;
                Vector2 Center = Owner.Center;
                Vector2 Top = Center + oldRotate[i].ToRotationVector2() * (oldLength[i] + trailTopWidth + oldDistanceToOwner[i]);
                Vector2 Bottom = Center + oldRotate[i].ToRotationVector2() * (oldLength[i] - ControlTrailBottomWidth(factor) + oldDistanceToOwner[i]);

                var topColor = Color.Lerp(new Color(238, 218, 130, alpha), new Color(167, 127, 95, 0), 1 - factor);
                var bottomColor = Color.Lerp(new Color(109, 73, 86, alpha), new Color(83, 16, 85, 0), 1 - factor);
                bars.Add(new((Top).ToVector3(), topColor, new Vector2(factor, 0)));
                bars.Add(new((Bottom).ToVector3(), bottomColor, new Vector2(factor, 1)));
            }

            if (bars.Count > 2)
            {
                LunarVeilUtils.DrawTrail(Main.graphics.GraphicsDevice, () =>
                {

                    Effect effect = LunarVeilUtils.SimpleGradientTrailFilter.GetShader().Shader;

                    effect.Parameters["transformMatrix"].SetValue(LunarVeilUtils.GetTransfromMaxrix());
                    effect.Parameters["sampleTexture"].SetValue(TrailTexture.Value);
                    effect.Parameters["gradientTexture"].SetValue(GradientTexture.Value);

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                        Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;
                        Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                    }
                }, BlendState.NonPremultiplied, SamplerState.PointWrap, RasterizerState.CullNone);
            }
        }
    }
}
