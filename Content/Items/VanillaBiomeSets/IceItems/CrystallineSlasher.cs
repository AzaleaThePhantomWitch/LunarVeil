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
        int AttackCounter = 1;
        int ComboAtt;
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
            //     Particle.NewParticle<SnowflakeParticle>(position, -velocity, Color.White);

            int dir = AttackCounter;
            AttackCounter = -AttackCounter;
           
            

            if (ComboAtt >= 6)
                ComboAtt = 0;

            int combo = ComboAtt;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);




            ComboAtt++;




            return false;
        }

    }

    public class CrystallineSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "LunarVeil/Content/Items/VanillaBiomeSets/IceItems/CrystallineSlasher";

        int slash1time = 0;

        ref float ComboAtt => ref Projectile.ai[0];
        private float Timer2;
        private ref float Dir2 => ref Projectile.ai[1];
        private Player Owner => Main.player[Projectile.owner];
        private float ExtraUpdates => 16;
        

        private float SwingRange2 = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
        private float SwingYRadius => 32;
        private float SwingXRadius => 128;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
        }

        public override void SetDefaults()
        {
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

                    break;

                case 2:

                    swingTime = 44;
                    SwingRange2 = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver2;

                    break;

                case 3:
                    swingTime = 52;
                    
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
                    swingTime = 60;
                    
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

                    swingTime = 80;
                    SwingRange2 = MathHelper.TwoPi + MathHelper.PiOver2 + MathHelper.PiOver2;

                    break;
            }


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
        public Vector2 ProjectileVelocityBeforeHit;
        public bool Hit;
        public bool Swinging = false;
        public bool Swinging2 = false;
        int Timer;
        protected override void SwingAI()
        {

            switch (ComboAtt)
            {
                case 0:
                    {
                        if (Timer > 0)
                        {
                            Projectile.timeLeft++;
                            Timer--;
                        }
                        Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                        float multiplier = 0.2f;
                        RGB *= multiplier;

                        Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                        int dir = (int)Projectile.ai[1];
                        float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);

                        //Smooth it some more
                        float swingProgress = Easing.InOutBack(lerpValue);

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

                    break;

                case 1:
                    {
                        if (Timer > 0)
                        {
                            Projectile.timeLeft++;
                            Timer--;
                        }


                        Timer2++;
                        if (Timer2 == 1)
                        {
                            for (int i = 0; i < Projectile.oldPos.Length; i++)
                            {
                                Projectile.oldPos[i] = Projectile.position;
                            }
                        }

                        float swingProgress2 = Timer2 / SwingTime;
                        float easedSwingProgress = Easing.InOutExpo(swingProgress2, 5f);
                        float targetRotation = Projectile.velocity.ToRotation();

                        int dir2 = (int)Dir2;

                        float xOffset;
                        float yOffset;
                        if (dir2 == 1)
                        {
                            xOffset = SwingXRadius * MathF.Sin(easedSwingProgress * SwingRange2 + SwingRange2);
                            yOffset = SwingYRadius * MathF.Cos(easedSwingProgress * SwingRange2 + SwingRange2);
                        }
                        else
                        {
                            xOffset = SwingXRadius * MathF.Sin((1f - easedSwingProgress) * SwingRange2 + SwingRange2);
                            yOffset = SwingYRadius * MathF.Cos((1f - easedSwingProgress) * SwingRange2 + SwingRange2);
                        }


                        Projectile.Center = Owner.Center + new Vector2(xOffset, yOffset).RotatedBy(targetRotation);
                        Projectile.rotation = (Projectile.Center - Owner.Center).ToRotation() + MathHelper.PiOver4;

                        Owner.heldProj = Projectile.whoAmI;
                        Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                        Owner.itemRotation = Projectile.rotation * Owner.direction;
                        Owner.itemTime = 2;
                        Owner.itemAnimation = 2;

                        // Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
                        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
                        Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

                        armPosition.Y += Owner.gfxOffY;


                    }


                    break;

                case 2:
                    {

                        if (Timer > 0)
                        {
                            Projectile.timeLeft++;
                            Timer--;
                        }


                        Timer2++;
                        if (Timer2 == 1)
                        {
                            for (int i = 0; i < Projectile.oldPos.Length; i++)
                            {
                                Projectile.oldPos[i] = Projectile.position;
                            }
                        }



                        float swingProgress2 = Timer2 / SwingTime;
                        float easedSwingProgress = Easing.InOutExpo(swingProgress2, 5f);
                        float targetRotation = Projectile.velocity.ToRotation();

                        int dir2 = (int)Dir2;

                        float xOffset;
                        float yOffset;
                        if (dir2 == 1)
                        {
                            xOffset = SwingXRadius * MathF.Sin(easedSwingProgress * SwingRange2 + SwingRange2);
                            yOffset = SwingYRadius * MathF.Cos(easedSwingProgress * SwingRange2 + SwingRange2);
                        }
                        else
                        {
                            xOffset = SwingXRadius * MathF.Sin((1f - easedSwingProgress) * SwingRange2 + SwingRange2);
                            yOffset = SwingYRadius * MathF.Cos((1f - easedSwingProgress) * SwingRange2 + SwingRange2);
                        }


                        Projectile.Center = Owner.Center + new Vector2(xOffset, yOffset).RotatedBy(targetRotation);
                        Projectile.rotation = (Projectile.Center - Owner.Center).ToRotation() + MathHelper.PiOver4;

                        Owner.heldProj = Projectile.whoAmI;
                        Owner.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
                        Owner.itemRotation = Projectile.rotation * Owner.direction;
                        Owner.itemTime = 2;
                        Owner.itemAnimation = 2;

                        // Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
                        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
                        Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

                        armPosition.Y += Owner.gfxOffY;

                    }


                    break;

                case 3:
                    {
                        if (Timer > 0)
                        {
                            Projectile.timeLeft++;
                            Timer--;
                        }
                        Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                        float multiplier = 0.2f;
                        RGB *= multiplier;

                        Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                        int dir = (int)Projectile.ai[1];
                        float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);

                        //Smooth it some more
                        float swingProgress = Easing.InExpo(lerpValue, 6);

                        // the actual rotation it should have
                        float defRot = Projectile.velocity.ToRotation();
                        // starting rotation

                        //How wide is the swing, in radians
                        float swingRange = MathHelper.PiOver2 + MathHelper.PiOver2;
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

                    break;

                case 4:
                    {
                        if (Timer > 0)
                        {
                            Projectile.timeLeft++;
                            Timer--;
                        }
                        Vector3 RGB = new Vector3(1.28f, 0f, 1.28f);
                        float multiplier = 0.2f;
                        RGB *= multiplier;

                        Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

                        int dir = (int)Projectile.ai[1];
                        float lerpValue = Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft, true);

                        //Smooth it some more
                        float swingProgress = Easing.InExpo(lerpValue, 6);

                        // the actual rotation it should have
                        float defRot = Projectile.velocity.ToRotation();
                        // starting rotation

                        //How wide is the swing, in radians
                        float swingRange = MathHelper.TwoPi + MathHelper.PiOver2;
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

                    break;
            }
        


        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Hit)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                Particle.NewParticle<IceStrikeParticle>(target.Center, Projectile.velocity *= 0f, Color.White);
                // Main.LocalPlayer.GetModPlayer<MyPlayer>().ShakeAtPosition(Projectile.Center, 1024f, 16f);
                for (int i = 0; i < 15; i++)
                {
                    //Particles Here
                }
                Hit = true;
                ProjectileVelocityBeforeHit = Projectile.velocity;
                Timer = 64;
            }


        }

    }
}