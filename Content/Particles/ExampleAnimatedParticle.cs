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
    internal class ExampleAnimatedParticle : Particle
    {
        public int FrameWidth = 270;
        public int FrameHeight = 249;
        public int MaxFrameCount = 5;
        public int FrameCounter = 0;
        public int TicksPerFrame = 4;
        public override void OnSpawn()
        {
            Frame = new Rectangle(0, 0, FrameWidth, FrameHeight);
        }

        public override void Update()
        {
            Velocity *= 0.98f;
            Rotation += 0.01f;
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
            if (fadeIn > 60 || color.A < 10)
                active = false;
        }
    }
}