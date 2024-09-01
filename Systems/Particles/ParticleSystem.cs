﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace LunarVeil.Systems.Particles
{
    public class ParticleSystem : ModSystem
    {
        //public static Particle[] Particles = new Particle[Coralite.MaxParticleCount];
        public static int MaxParticleCount => 500;
        public static List<Particle> Particles = new(MaxParticleCount);

        public static Asset<Texture2D>[] ParticleAssets;

        public override void Load()
        {
            base.Load();
            On_Main.DrawDust += DrawMainParticles;
        }

        public override void PostAddRecipes()
        {
            if (Main.dedServ)
                return;

            ParticleAssets = new Asset<Texture2D>[ParticleLoader.ParticleCount];

            for (int i = 0; i < ParticleLoader.ParticleCount; i++)
            {
                Particle particle = ParticleLoader.GetParticle(i);
                if (particle != null)
                    ParticleAssets[i] = ModContent.Request<Texture2D>(particle.Texture);
            }
        }

        public override void Unload()
        {
            On_Main.DrawDust -= DrawMainParticles;
            ParticleLoader.Unload();

            ParticleAssets = null;
            Particles = null;
        }

        public override void PostUpdateDusts()
        {
            UpdateParticle();
        }

        /// <summary>
        /// 更新粒子
        /// </summary>
        public static void UpdateParticle()
        {
            if (Main.netMode == NetmodeID.Server)//不在服务器上运行
                return;
            if (Main.gameInactive)//不在游戏暂停时运行
                return;

            for (int i = 0; i < Particles.Count; i++)
            {
                Particle particle = Particles[i];

                if (particle == null)
                    continue;

                try
                {
                    particle.Update();
                    if (particle.ShouldUpdateCenter())
                        particle.Center += particle.Velocity;

                    //在粒子不活跃时把一些东西释放掉
                    if (!particle.active)
                    {
                        particle.oldCenter = null;
                        particle.oldRot = null;
                    }

                    //一些防止粒子持续时间过长的措施，额...还是建议在update里手动设置active比较好
                    if (particle.shouldKilledOutScreen && !LunarVeilUtils.OnScreen(particle.Center - Main.screenPosition))
                        particle.active = false;

                    if (particle.Scale < 0.001f)
                        particle.active = false;

                    if (particle.fadeIn > 1000)
                        particle.active = false;
                }
                catch (System.Exception)
                {
                    particle.active = false;
                }
            }

            Particles.RemoveAll(p => p == null || !p.active);
           
        }

        private void DrawMainParticles(On_Main.orig_DrawDust orig, Main self)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.TransformationMatrix);
            DrawParticles(spriteBatch);
            spriteBatch.End();
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            ArmorShaderData armorShaderData = null;
            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles[i];
                if (particle == null || !particle.active)
                    continue;

                if (!LunarVeilUtils.OnScreen(particle.Center - Main.screenPosition))
                    continue;

                if (particle.shader != armorShaderData)
                {
                    spriteBatch.End();
                    armorShaderData = particle.shader;
                    if (armorShaderData == null)
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.TransformationMatrix);
                    else
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
                        particle.shader.Apply(null);
                    }
                }

                particle.Draw(spriteBatch);
            }

            if (armorShaderData != null)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}