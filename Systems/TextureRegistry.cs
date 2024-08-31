﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace LunarVeil.Systems
{
    internal static class TextureRegistry
    {

        public static Asset<Texture2D> NoiseTextureClouds => ModContent.Request<Texture2D>("LunarVeil/Assets/NoiseTextures/Clouds");
    }
}