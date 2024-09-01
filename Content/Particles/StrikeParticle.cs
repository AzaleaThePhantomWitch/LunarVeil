﻿using LunarVeil.Systems.Particles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LunarVeil.Content.Particles
{
    internal class StrikeParticle : Particle
    {
        public int FrameWidth = 128;
        public int FrameHeight = 128;
        public int MaxFrameCount = 8;
        public int FrameCounter = 0;
        public int TicksPerFrame = 4;
        public override void OnSpawn()
        {
            Rotation += Main.rand.NextFloat(-MathHelper.TwoPi, MathHelper.TwoPi);
            Frame = new Rectangle(0, 0, FrameWidth, FrameHeight);
        }

        public override void Update()
        {
            Velocity *= 0.98f;
            Scale *= 0.997f;
            color *= 0.99f;

            FrameCounter++;
            if(FrameCounter >= TicksPerFrame)
            {
                Frame.Y += FrameHeight;
                if(Frame.Y >= FrameHeight * MaxFrameCount)
                {
                    Frame.Y = 0;
                }
                FrameCounter = 0;
            }

            fadeIn++;
            if (fadeIn > 30 || color.A < 5)
                active = false;
        }
    }
}