using LunarVeil.Systems;
using LunarVeil.Systems.Skies;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace LunarVeil
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class LunarVeil : Mod
    {

        public LunarVeil()
        {
            Instance = this;
        }

        public static LunarVeil Instance;
        public override void Load()
        {
            LunarVeilUtils.LoadShaders();
            LunarVeilUtils.LoadOrderedLoadables();
            Instance = this;
        }

        public override void Unload()
        {
            LunarVeilUtils.UnloadOrderedLoadables();
        }
    }
}
