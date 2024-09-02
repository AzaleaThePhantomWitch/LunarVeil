using LunarVeil.Content.Bases;
using LunarVeil.Systems.MiscellaneousMath;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using LunarVeil.Content.Particles;
using LunarVeil.Systems.Particles;
using LunarVeil.Systems.Players;
using System;

namespace LunarVeil.Content.Items.VanillaBiomeSets.IceItems
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod

    
    public class CrystallineSlasher : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.LunarVeil.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.useTime = 126;
            Item.useAnimation = 126;
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
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 100;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);

            comboPlayer.IncreaseCombo(maxCombo: 6);
            return false;
        }
    }

    public class CrystallineSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "LunarVeil/Content/Items/VanillaBiomeSets/IceItems/CrystallineSlasher";
        ref float ComboAtt => ref Projectile.ai[0];

        private float SwingRange2 = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
        private float SwingYRadius = 64 /1.5f;
        private float SwingXRadius = 128/1.5f;
        private bool initSwingAI;
        public bool Hit;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
        }

        public override void SetDefaults()
        {
          
      

            //           swingTime /= 2;

            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 38;
            Projectile.width = 38;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = Swing_Speed_Multiplier - 1;
         
            Projectile.usesLocalNPCImmunity = true;

            //Multiplying by the thing so it's still 10 ticks
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
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
    


        private void InitializeSwingAI()
        {
            if (initSwingAI)
                return;
            initSwingAI = true;
            //Set swing variables here
            //How long the swing lasts in ticks
            switch (ComboAtt)
            {
                case 0:
                    swingTime = 34;

                    //How far the trail extends upward (away the player)
                    trailTopWidth = 10;

                    //How far the trail extends downward (towards the player)
                    trailBottomWidth = 250;

                    //The number of points in the trail, higher means the trail is longer
                    trailCount = 65;

                    //The distance from the player where the trail starts
                    distanceToOwner = 0;

                    //Brightness/Transparency of trail, 255 is fully opaque
                    alpha = 200;
                    break;

                case 1:
                    swingTime = 34;
                    trailBottomWidth = 150;
                    trailTopWidth = 5;
                    break;

                case 2:
                    trailTopWidth = 5;

                    swingTime = 34;
                    trailBottomWidth = 150;
                    SwingRange2 = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver2;
                    break;

                case 3:
                    swingTime = 34;

                    //How far the trail extends upward (away the player)
                    trailTopWidth = 10;

                    //How far the trail extends downward (towards the player)
                    trailBottomWidth = 250;

                    //The number of points in the trail, higher means the trail is longer
                    trailCount = 65;

                    //The distance from the player where the trail starts
                    distanceToOwner = 0;

                    //Brightness/Transparency of trail, 255 is fully opaque
                    alpha = 200;
                    break;

                case 4:
                    swingTime = 34;

                    //How far the trail extends upward (away the player)
                    trailTopWidth = 10;

                    //How far the trail extends downward (towards the player)
                    trailBottomWidth = 250;

                    //The number of points in the trail, higher means the trail is longer
                    trailCount = 65;

                    //The distance from the player where the trail starts
                    distanceToOwner = 0;

                    //Brightness/Transparency of trail, 255 is fully opaque
                    alpha = 200;

                    break;

                case 5:
                    SwingYRadius = 32;
                    SwingXRadius = 170;
                    swingTime = 60;
                    trailBottomWidth = 100;
                    SwingRange2 = MathHelper.TwoPi + MathHelper.PiOver2 + MathHelper.PiOver4;
                  //  windUpTime = 20 * Swing_Speed_Multiplier;

                    break;
            }
            Projectile.timeLeft = SwingTime;
            Projectile.localNPCHitCooldown = Projectile.timeLeft;
        }
        protected override void SwingAI()
        {
            InitializeSwingAI();
            switch (ComboAtt)
            {
                case 0:               
                    SimpleEasedSwingAI(EaseFunction.EaseInOutBack);
                    break;

                case 1:
                    OvalEasedSwingAI(EaseFunction.EaseInOutExpo, SwingXRadius + 20, SwingYRadius - 20, SwingRange2);
                    break;

                case 2:
                    OvalEasedSwingAI(EaseFunction.EaseInOutExpo, SwingXRadius + 20, SwingYRadius - 20, SwingRange2);
                    break;

                case 3:                
                    SimpleEasedSwingAI(EaseFunction.EaseInOutBack);    
                    break;

                case 4:               
                    SimpleEasedSwingAI(EaseFunction.EaseInOutBack);
                    break;

                case 5:
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
                    float swingRange = MathHelper.PiOver2 + MathHelper.PiOver4 + MathHelper.TwoPi;
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
                    //OvalEasedSwingAI(EaseFunction.EaseInOutCirc, SwingXRadius, SwingYRadius, SwingRange2);
                    break;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Hit)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                
                Particle.NewParticle<IceStrikeParticle>(target.Center,Vector2.Zero, Color.White);
                // Main.LocalPlayer.GetModPlayer<MyPlayer>().ShakeAtPosition(Projectile.Center, 1024f, 16f);
                for (int i = 0; i < 15; i++)
                {
                    //Particles Here
                }
                Hit = true;
                hitstopTimer = 6 * Swing_Speed_Multiplier;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            /*
            switch (ComboAtt)
            {
                case 1:
                case 2:
                case 5:
                    DrawSlashTrail();
                    float rot = Projectile.rotation;
                    float x = (float)Math.Cos(-rot) * 60;

                    Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
                    Player owner = Main.player[Projectile.owner];

                    var target = new Rectangle(
                        (int)(owner.Center.X - Main.screenPosition.X), 
                        (int)(owner.Center.Y - Main.screenPosition.Y), 
                        (int)Math.Abs(x / 120 * tex.Size().Length()), 80);

                    Main.spriteBatch.Draw(tex, target, null, lightColor, rot, new Vector2(0, tex.Height), SpriteEffects.None, default);

                    return false;
            }
        */
            return base.PreDraw(ref lightColor);
        }
    }
}