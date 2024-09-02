using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using LunarVeil.Systems.Primitives;
using Terraria.DataStructures;
using LunarVeil.Content.Dusts;
using LunarVeil.Systems.MiscellaneousMath;
using LunarVeil.Systems;
using ReLogic.Content;
using Terraria.Graphics.Shaders;


namespace LunarVeil.Content.Items.ModdedBiomeSets.SoulPrisonItems
{

   
        public class CometRideGreatsword : ModItem
        {
            private int cooldown = 0;

            

            public override void SetStaticDefaults()
            {
               
            }

            public override void SetDefaults()
            {
                Item.damage = 28;
                Item.DamageType = DamageClass.Melee;
                Item.width = 36;
                Item.height = 38;
            Item.useTime = 5;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Swing;
                Item.knockBack = 7.5f;
                Item.value = Item.sellPrice(gold: 1);
                Item.rare = ItemRarityID.Green;
                Item.UseSound = SoundID.Item1;
                Item.shootSpeed = 14f;
                Item.autoReuse = false;
                Item.shoot = ModContent.ProjectileType<PhantasmalCometProj>();
                Item.useTurn = true;
            }


            public override void HoldItem(Player Player)
            {
                cooldown--;
                base.HoldItem(Player);
            }

            public override void UseItemHitbox(Player Player, ref Rectangle hitbox, ref bool noHitbox)
            {
                if (Main.rand.NextBool(15))
                {
                    Dust.NewDustPerfect(hitbox.TopLeft() + new Vector2(Main.rand.NextFloat(hitbox.Width), Main.rand.NextFloat(hitbox.Height)),
                    ModContent.DustType<Dusts.GlowDust>(), new Vector2(Main.rand.NextFloat(-0.3f, 1.2f) * Player.direction, -Main.rand.NextFloat(0.3f, 0.5f)), 0,
                    new Color(Main.rand.NextFloat(0.15f, 0.30f), Main.rand.NextFloat(0.2f, 0.30f), Main.rand.NextFloat(0.3f, 0.5f), 0f), Main.rand.NextFloat(0.15f, 0.40f));
                }
            }

            public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
            {
                var direction = new Vector2(0, -1);
                direction = direction.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
                position = Main.MouseWorld + direction * 800;

                direction *= -10;
                velocity = direction;
                damage = (int)(damage * 1.5f);
            }

            public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
            {

                return true;
            }


        }

        public class PhantasmalCometProj : ModProjectile, IDrawPrimitive, IDrawAdditive
    {
        private List<Vector2> cache;
        private Trail trail;
        private Trail trail2;

        private float trailWidth = 1;
        private bool stuck = false;

   

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 50;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 6;
        }

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moonfury Spike");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.ai[0] = Main.MouseWorld.Y;

            if (Projectile.Bottom.Y > Projectile.ai[0])
                Projectile.tileCollide = true;
            else
                Projectile.tileCollide = false;

            if (!stuck)
            {
                

                var da = Dust.NewDustPerfect(Projectile.Bottom + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(15), ModContent.DustType<Dusts.TSmokeDust>(), Vector2.Zero, 0, new Color(20, 140, 250), 0.3f);
                da.customData = Main.rand.NextFloat(0.6f, 1.3f);
                da.fadeIn = 10;
                Dust.NewDustPerfect(Projectile.Bottom + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(15), ModContent.DustType<Dusts.GlowDust>(), Vector2.Zero, 0, new Color(50, 200, 255), 0.4f).fadeIn = 10;
                ManageCaches();

                Projectile.rotation = Projectile.velocity.ToRotation() - 1.44f;
            }
            else
            {
                Projectile.friendly = false;

                if (Projectile.timeLeft <= 30)
                    Projectile.alpha += 10;

                trailWidth *= 0.93f;

                if (trailWidth > 0.05f)
                    trailWidth -= 0.05f;
                else
                    trailWidth = 0;
            }

            ManageTrail();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!stuck)
            {

                //add hit sound effect here
                for (int k = 0; k <= 8; k++)
                {
                    Vector2 pos = Projectile.Bottom + Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(15);
                    Vector2 velocity = (-Vector2.UnitY).RotatedByRandom(0.7f) * Main.rand.NextFloat(1f, 2f);
                    Dust.NewDustPerfect(pos, ModContent.DustType<Dusts.GlowDust>(), velocity, 0, new Color(50, 180, 255), Main.rand.NextFloat(0.4f, 0.8f)).fadeIn = 10;

                    if (Main.rand.NextBool(3))
                    {
                        Dust.NewDustPerfect(Projectile.TopLeft + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)),
                        ModContent.DustType<Dusts.GlowDust>(), new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.2f, 0.4f)), 0,
                        new Color(Main.rand.NextFloat(0.25f, 0.30f), Main.rand.NextFloat(0.25f, 0.30f), Main.rand.NextFloat(0.35f, 0.45f), 0f), Main.rand.NextFloat(0.2f, 0.4f));
                    }
                }

                //CameraSystem.shake += 10;
               // Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Bottom, Vector2.Zero, ModContent.ProjectileType<GravediggerSlam>(), 0, 0, Projectile.owner).timeLeft = 194;
               // Terraria.Audio.SoundEngine.PlaySound(SoundID.Item96, Projectile.Center);
                stuck = true;
                Projectile.extraUpdates = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 1;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
               ModContent.ProjectileType<PhantasmalRingExplosion>(), Projectile.damage * 2, 0f, Projectile.owner, 0f, 0f);

                for (int i = 0; i < 16; i++)
                {
                    Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.GlowStarDust>(), Vector2.UnitX.RotatedBy(Main.rand.NextFloat(6.28f)) * Main.rand.NextFloat(12), 0, new Color(50, 250, 255), 0.4f);
                }

            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 pos = Projectile.Bottom + new Vector2(0, 20) - Main.screenPosition;
            Main.spriteBatch.Draw(tex, pos, null, lightColor * (1 - Projectile.alpha / 255f), Projectile.rotation, new Vector2(tex.Width / 2, tex.Height), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
           // target.AddBuff(ModContent.BuffType<MoonfuryDebuff>(), 150);
        }

        private void ManageCaches()
        {
            if (cache == null)
            {
                cache = new List<Vector2>();

                for (int i = 0; i < 50; i++)
                {
                    cache.Add(Projectile.Bottom + new Vector2(0, 20));
                }
            }

            if (Projectile.oldPos[0] != Vector2.Zero)
                cache.Add(Projectile.oldPos[0] + new Vector2(Projectile.width / 2, Projectile.height) + new Vector2(0, 20));

            while (cache.Count > 50)
            {
                cache.RemoveAt(0);
            }
        }

        private void ManageTrail()
        {
            if (trail is null || trail.IsDisposed)
                trail = new Trail(Main.instance.GraphicsDevice, 50, new RoundedTip(12), factor => (10 + factor * 25) * trailWidth, factor => new Color(20 + (int)(100 * factor.X), 255 , 255) * factor.X * trailWidth);

            trail.Positions = cache.ToArray();

            if (trail2 is null || trail2.IsDisposed)
                trail2 = new Trail(Main.instance.GraphicsDevice, 50, new RoundedTip(6), factor => (80 + 0 + factor * 0) * trailWidth, factor => new Color(10 + (int)(100 * factor.X), 255, 255) * factor.X * 0.15f * trailWidth);

            trail2.Positions = cache.ToArray();

            if (Projectile.velocity.Length() > 1)
            {
                trail.NextPosition = Projectile.Bottom + new Vector2(0, 20) + Projectile.velocity;
                trail2.NextPosition = Projectile.Bottom + new Vector2(0, 20) + Projectile.velocity;
            }
        }

        public void DrawPrimitives()
        {
            Effect effect = Filters.Scene["LunarVeil:CometTrail"].GetShader().Shader;

            var world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["time"].SetValue(Main.GameUpdateCount * 0.02f);
            effect.Parameters["repeats"].SetValue(8f);
            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/GlowTrail").Value);
            effect.Parameters["sampleTexture2"].SetValue(ModContent.Request<Texture2D>("LunarVeil/Assets/Trails/CrystalTrail").Value);

            trail?.Render(effect);

            effect.Parameters["sampleTexture2"].SetValue(TextureAssets.MagicPixel.Value);

            trail2?.Render(effect);
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Additive").Value;
            Color color = Color.White * (1 - Projectile.alpha / 255f);
            spriteBatch.Draw(tex, Projectile.Bottom + new Vector2(0, 20) - Main.screenPosition, null, color * 0.5f, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height), Projectile.scale, SpriteEffects.None, 0);
        }
    }


    internal class PhantasmalRingExplosion : ModProjectile,
    IPixelPrimitiveDrawer
    {
        //Texture
        public override string Texture => TextureRegistry.EmptyTexturePath;

        //AI
        private float LifeTime => 32f;
        private ref float Timer => ref Projectile.ai[0];
        private float Progress
        {
            get
            {
                float p = Timer / LifeTime;
                return MathHelper.Clamp(p, 0, 1);
            }
        }

        //Draw Code
        private PrimitiveTrailCopy BeamDrawer;
        private int DrawMode;
        private bool SpawnDustCircle;

        //Trailing
        private Asset<Texture2D> FrontTrailTexture => TrailRegistry.WaterTrail;
        private MiscShaderData FrontTrailShader => TrailRegistry.LaserShader;

        private Asset<Texture2D> BackTrailTexture => TrailRegistry.SimpleTrail;
        private MiscShaderData BackTrailShader => TrailRegistry.LaserShader;

        //Radius
        private float StartRadius => 4;
        private float EndRadius => 44;
        private float Width => 44;

        //Colors
        private Color FrontCircleStartDrawColor => Color.White;
        private Color FrontCircleEndDrawColor => Color.Turquoise;
        private Color BackCircleStartDrawColor => Color.Lerp(Color.White, Color.Turquoise, 0.4f);
        private Color BackCircleEndDrawColor => Color.Lerp(Color.Turquoise, Color.Black, 0.7f);
        private Vector2[] CirclePos;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.timeLeft = (int)LifeTime;
            Projectile.tileCollide = false;

            //Points on the circle
            CirclePos = new Vector2[64];
        }

        public override void AI()
        {
            Timer++;
            AI_ExpandCircle();
            AI_DustCircle();
        }

        private void AI_ExpandCircle()
        {
            float easedProgess = Easing.OutCirc(Progress);
            float radius = MathHelper.Lerp(StartRadius, EndRadius, easedProgess);
            DrawCircle(radius);
        }

        private void AI_DustCircle()
        {
            if (!SpawnDustCircle && Timer >= 15)
            {
                for (int i = 0; i < 48; i++)
                {
                    Vector2 rand = Main.rand.NextVector2CircularEdge(EndRadius, EndRadius);
                    Vector2 pos = Projectile.Center + rand;
                    Dust d = Dust.NewDustPerfect(pos, ModContent.DustType<GlowDust>(), Vector2.Zero,
                        newColor: BackCircleStartDrawColor,
                        Scale: Main.rand.NextFloat(0.1f, 0.3f));
                    d.noGravity = true;
                }
                SpawnDustCircle = true;
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        private void DrawCircle(float radius)
        {
            Vector2 startDirection = Vector2.UnitY;
            for (int i = 0; i < CirclePos.Length; i++)
            {
                float circleProgress = i / (float)CirclePos.Length;
                float radiansToRotateBy = circleProgress * (MathHelper.TwoPi + MathHelper.PiOver4 / 2);
                CirclePos[i] = Projectile.Center + startDirection.RotatedBy(radiansToRotateBy) * radius;
            }
        }

        public float WidthFunction(float completionRatio)
        {
            float width = Width;
            float startExplosionScale = 4f;
            float endExplosionScale = 0f;
            float easedProgess = Easing.OutCirc(Progress);
            float scale = MathHelper.Lerp(startExplosionScale, endExplosionScale, easedProgess);
            switch (DrawMode)
            {
                default:
                case 0:
                    return Projectile.scale * scale * width * Easing.SpikeInOutCirc(Progress);
                case 1:
                    return Projectile.scale * width * 2.2f * Easing.SpikeInOutCirc(Progress);

            }
        }

        public Color ColorFunction(float completionRatio)
        {
            switch (DrawMode)
            {
                default:
                case 0:
                    //Front Trail
                    return Color.Transparent;
                case 1:
                    //Back Trail
                    return Color.Transparent;
            }
        }

        public void DrawPixelPrimitives(SpriteBatch spriteBatch)
        {
            BeamDrawer ??= new PrimitiveTrailCopy(WidthFunction, ColorFunction, null, true, TrailRegistry.LaserShader);
            float easedProgess = Easing.OutCubic(Progress);

            //Back Trail   
            DrawMode = 1;
            BeamDrawer.SpecialShader = BackTrailShader;
            BeamDrawer.SpecialShader.UseColor(
                Color.Lerp(BackCircleStartDrawColor, BackCircleEndDrawColor, easedProgess));
            BeamDrawer.SpecialShader.SetShaderTexture(BackTrailTexture);
            BeamDrawer.DrawPixelated(CirclePos, -Main.screenPosition, CirclePos.Length);

            //Front Trail
            DrawMode = 0;
            BeamDrawer.SpecialShader = FrontTrailShader;
            BeamDrawer.SpecialShader.UseColor(Color.Lerp(FrontCircleStartDrawColor, FrontCircleEndDrawColor,
                Easing.OutCirc(Progress)));
            BeamDrawer.SpecialShader.SetShaderTexture(FrontTrailTexture);
            BeamDrawer.DrawPixelated(CirclePos, -Main.screenPosition, CirclePos.Length);
            Main.spriteBatch.ExitShaderRegion();
        }
    }
}

