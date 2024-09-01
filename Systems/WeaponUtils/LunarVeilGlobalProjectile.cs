using LunarVeil.Systems.Particles;
using LunarVeil.Systems.WeaponUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LunarVeil.Systems.WeaponUtils
{
    public class LunarVeilGlobalProjectile : GlobalProjectile
    {
        // Life is pain
        public override bool InstancePerEntity => true;
        public bool IsBullet { get; private set; } = false;
        public bool IsArrow { get; private set; } = false;
        public bool IsPowerShot = false;
        public bool IsPowerShotSniper = false;
        public bool IsMirageArrow = false;

        public bool slowedTime = false;

        public bool RetractSlow = false;
        private bool HasRebounded = false;
        private bool PracticeTargetHit = false;

        private bool Undertaker = false;
        private int UndertakerCounter = 0;
        private bool WildEyeCrit = false;

      
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;

            if (IsMirageArrow)
            {
                Main.spriteBatch.Reload(BlendState.Additive, SpriteSortMode.Immediate);

                int shaderID = GameShaders.Armor.GetShaderIdFromItemId(ItemID.MirageDye);

                DrawData newData = new DrawData(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, texture.Size() / 2f, projectile.scale, 0, 0);
                GameShaders.Armor.Apply(shaderID, projectile, newData);
                newData.Draw(Main.spriteBatch);

                Main.spriteBatch.Reload(BlendState.AlphaBlend, SpriteSortMode.Deferred);

                return false;
            }

            return base.PreDraw(projectile, ref lightColor);
        }


        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner]; 

            if (IsPowerShot)
            {
                modifiers.SourceDamage *= 1.25f;
            }


            #region Accessories
           
            #endregion
        }

        public Entity ownerEntity { get; private set; }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_Parent parent)
            {
                ownerEntity = parent.Entity;
                /*if (parent.Entity is NPC npc)
                    Main.NewText("from npc " + npc.FullName);

                if (parent.Entity is Player player)
                    Main.NewText("from player " + player.name);*/
            }

        


            if (source != null && source.Context != null)
            {
                string[] sourceInfo = source.Context.ToString().Split('_');
                string sourceType = sourceInfo[0];

                string sourceAction = "none";
                if (sourceInfo.Length > 1) sourceAction = sourceInfo[1];

                if (source is EntitySource_ItemUse_WithAmmo)
                {

                    if (sourceType == "HeldBow")
                    {
                        IsArrow = true;
                    }
                }
            }

            // Vanilla arrows default to a rotation of 0 when initially spawned, I don't know why
            if (IsArrow)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }

       

        public override bool PreAI(Projectile projectile)
        {
            if (slowedTime && !projectile.friendly)
            {
                projectile.position -= projectile.velocity * 0.95f;
            }

            if (Undertaker && UndertakerCounter < 15) UndertakerCounter++;

            return base.PreAI(projectile);
        }

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == 590)
            {
                projectile.timeLeft = 100;
            }
        }
        /*
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
          

            if (player.GetModPlayer<OvermorrowModPlayer>().PracticeTarget && IsArrow && !PracticeTargetHit)
            {
                SpawnPracticeTargetFail(player);
            }

          
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
     

            if (player.GetModPlayer<OvermorrowModPlayer>().PracticeTarget && IsArrow && !PracticeTargetHit)
            {
                SpawnPracticeTargetHit(player);
            }

           

            
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.player[projectile.owner];
          

            return base.OnTileCollide(projectile, oldVelocity);
        }

        private void SpawnPracticeTargetHit(Player player)
        {
            BowPlayer bowPlayer = player.GetModPlayer<BowPlayer>();

            if (bowPlayer.PracticeTargetCounter < 5)
            {
                bowPlayer.PracticeTargetCounter++;
                Projectile.NewProjectile(null, player.Center + new Vector2(-4, -32), Vector2.Zero, ModContent.ProjectileType<Content.Items.Accessories.PracticeTarget.PracticeTargetIcon>(), 0, 0f, player.whoAmI);
            }

            PracticeTargetHit = true;
        }

        private void SpawnPracticeTargetFail(Player player)
        {
            BowPlayer bowPlayer = player.GetModPlayer<BowPlayer>();

            int failCount = bowPlayer.PracticeTargetCounter == 0 ? -1 : bowPlayer.PracticeTargetCounter;

            Projectile.NewProjectile(null, player.Center + new Vector2(-4, -32), Vector2.Zero, ModContent.ProjectileType<Content.Items.Accessories.PracticeTarget.PracticeTargetIcon>(), 0, 0f, player.whoAmI, 0f, failCount);
            bowPlayer.PracticeTargetCounter = 0;
        }
        */
        public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
        {
            if (RetractSlow)
            {
                speed = 4;
            }

            base.GrappleRetreatSpeed(projectile, player, ref speed);
        }

    }
}