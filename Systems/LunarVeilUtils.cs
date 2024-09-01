using LunarVeil.Systems.Particles;
using LunarVeil.Systems.Skies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace LunarVeil.Systems
{
    internal static class LunarVeilUtils
    {
        private static List<IOrderedLoadable> _loadCache;
        public static MiscShaderData CloudsShader => GameShaders.Misc["LunarVeil:Clouds"];
        public static MiscShaderData CloudsFrontShader => GameShaders.Misc["LunarVeil:CloudsFront"];
        public static MiscShaderData NightCloudsShader => GameShaders.Misc["LunarVeil:NightClouds"];
        public static MiscShaderData CloudsDesertShader => GameShaders.Misc["LunarVeil:CloudsDesert"];
        public static MiscShaderData CloudsDesertNightShader => GameShaders.Misc["LunarVeil:CloudsDesertNight"];
        public static Filter WaterFilter => Filters.Scene["LunarVeil:Water"];
        public static Filter WaterBasicFilter => Filters.Scene["LunarVeil:WaterBasic"];
        public static Filter LavaFilter => Filters.Scene["LunarVeil:Lava"];
        public static MiscShaderData GradientShader => GameShaders.Misc["LunarVeil:Gradient"];
        public static Filter CloudySkyFilter => Filters.Scene["LunarVeil:CloudySky"];
        public static Filter DesertSkyFilter => Filters.Scene["LunarVeil:DesertSky"];
        public static AssetRepository Assets => LunarVeil.Instance.Assets;
        private static string GlowingDustShader => "Stellamod:GlowingDust";
        public static MiscShaderData MiscGlowingDust => GameShaders.Misc[GlowingDustShader];

        public static void LoadShaders()
        {

            Asset<Effect> glowingDustShader = Assets.Request<Effect>("Assets/Effects/GlowingDust");
            GameShaders.Misc[LunarVeilUtils.GlowingDustShader] = new MiscShaderData(glowingDustShader, "GlowingDustPass");

            Asset<Effect> miscShader = Assets.Request<Effect>("Assets/Effects/Clouds", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:Clouds"] = new MiscShaderData(miscShader, "ScreenPass");

            Asset<Effect> miscShader2 = Assets.Request<Effect>("Assets/Effects/CloudsFront", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsFront"] = new MiscShaderData(miscShader2, "ScreenPass");

            Asset<Effect> miscShader3 = Assets.Request<Effect>("Assets/Effects/NightClouds", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:NightClouds"] = new MiscShaderData(miscShader3, "ScreenPass");

            Asset<Effect> miscShader4 = Assets.Request<Effect>("Assets/Effects/CloudsDesert", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsDesert"] = new MiscShaderData(miscShader4, "ScreenPass");

            Asset<Effect> miscShader5 = Assets.Request<Effect>("Assets/Effects/CloudsDesertNight", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:CloudsDesertNight"] = new MiscShaderData(miscShader5, "ScreenPass");

            var miscShader6 = new Ref<Effect>(LunarVeil.Instance.Assets.Request<Effect>("Assets/Effects/Water", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:Water"] = new Filter(new ScreenShaderData(miscShader6, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:Water"].Load();

            var miscShader7 = new Ref<Effect>(LunarVeil.Instance.Assets.Request<Effect>("Assets/Effects/WaterBasic", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:WaterBasic"] = new Filter(new ScreenShaderData(miscShader7, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:WaterBasic"].Load();


            var miscShader8 = new Ref<Effect>(LunarVeil.Instance.Assets.Request<Effect>("Assets/Effects/Lava", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["LunarVeil:Lava"] = new Filter(new ScreenShaderData(miscShader8, "PrimitivesPass"), EffectPriority.VeryHigh);
            Filters.Scene["LunarVeil:Lava"].Load();

            Asset<Effect> gradient = Assets.Request<Effect>("Assets/Effects/Gradient", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["LunarVeil:Gradient"] = new MiscShaderData(gradient, "ScreenPass");

            SkyManager.Instance["LunarVeil:CloudySky"] = new CloudySky();
            SkyManager.Instance["LunarVeil:CloudySky"].Load();
            Filters.Scene["LunarVeil:CloudySky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);


            SkyManager.Instance["LunarVeil:DesertSky"] = new DesertSky();
            SkyManager.Instance["LunarVeil:DesertSky"].Load();
            Filters.Scene["LunarVeil:DesertSky"] = new Filter((new ScreenShaderData("FilterMiniTower")).UseColor(0f, 0f, 0f).UseOpacity(0f), EffectPriority.VeryHigh);



        }

        public static void LoadOrderedLoadables()
        {
            _loadCache = new List<IOrderedLoadable>();
            foreach (Type type in LunarVeil.Instance.Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(IOrderedLoadable)))
                {
                    object instance = Activator.CreateInstance(type);
                    _loadCache.Add(instance as IOrderedLoadable);
                }

                _loadCache.Sort((n, t) => n.Priority.CompareTo(t.Priority));
            }

            for (int k = 0; k < _loadCache.Count; k++)
            {
                _loadCache[k].Load();
            }
        }

        public static void UnloadOrderedLoadables()
        {
            if (_loadCache != null)
            {
                foreach (IOrderedLoadable loadable in _loadCache)
                {
                    loadable.Unload();
                }

                _loadCache = null;
            }
            else
            {
             //   Logger.Warn("load cache was null, IOrderedLoadable's may not have been unloaded...");
            }
        }
        public static int ParticleType<T>() where T : Particle => ModContent.GetInstance<T>()?.Type ?? 0;

        public static bool OnScreen(Vector2 pos) => pos.X > -16 && pos.X < Main.screenWidth + 16 && pos.Y > -16 && pos.Y < Main.screenHeight + 16;

        public static bool OnScreen(Rectangle rect) => rect.Intersects(new Rectangle(0, 0, Main.screenWidth, Main.screenHeight));

        public static bool OnScreen(Vector2 pos, Vector2 size) => OnScreen(new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y));
    }
}
