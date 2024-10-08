﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.WorldBuilding;

namespace LunarVeil.WorldGeneration.BaseEdits
{
    internal class BrokenCircleShape : GenShape
    {
        private int _radius;
        private float _distortionAmount;
        public BrokenCircleShape(int radius,float distortionAmount) : base()
        {
            _radius = radius;
            _distortionAmount = distortionAmount;
        }

        public static float AperiodicSin(float x, float dx = 0f, float a = MathHelper.Pi, float b = MathHelper.E)
        {
            return (float)(Math.Sin(x * a + dx) + Math.Sin(x * b + dx)) * 0.5f;
        }

        public override bool Perform(Point origin, GenAction action)
        {
            float offsetAngle = WorldGen.genRand.NextFloat(-10f, 10f);
            for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.Pi * 0.0084f)
            {
                float distortionAmt = AperiodicSin(f, offsetAngle, MathHelper.PiOver2, MathHelper.E * 0.5f) * _distortionAmount;
                int radius = (int)(_radius - distortionAmt * _radius);
                if (radius <= 0)
                    continue;

                float ax = (int)MathF.Sin(f) * radius;
                float ay = (int)MathF.Cos(f) * radius;
                for (int dx = 0; dx != ax; dx += Math.Sign(ax))
                {
                    for (int dy = 0; dy != ay; dy += Math.Sign(ay))
                        UnitApply(action, origin, origin.X + dx, origin.Y + dy, new object[0]);
                }
            }

            return true;
        }
    }

    internal class FastNoiseShape : GenShape
    {
        private int _radius;
        private float _threshold = 0f;
        private FastNoiseLite _fastNoiseLite;
        public FastNoiseShape(int radius, float threshold = 0.0f, float frequency = 0.01f, FastNoiseLite.NoiseType noiseType = FastNoiseLite.NoiseType.Perlin, int seed = 1337) : base()
        {
            _radius = radius;
            _threshold = threshold;
            _fastNoiseLite = new FastNoiseLite(seed);
            _fastNoiseLite.SetFrequency(frequency);
            _fastNoiseLite.SetSeed(seed);
            _fastNoiseLite.SetNoiseType(noiseType);
        }

        public override bool Perform(Point origin, GenAction action)
        {
            void Apply(int x, int y)
            {
                if(_fastNoiseLite.GetNoise(origin.X + x, origin.Y + y) < _threshold)
                {
                    UnitApply(action, origin, origin.X + x, origin.Y + y, new object[0]);
                }
            }


            for(int i = -_radius; i < _radius; i++)
            {
                for(int j = -_radius; j < _radius; j++)
                {
                    Apply(i, j);
                }
            }

            return true;
        }
    }
}
