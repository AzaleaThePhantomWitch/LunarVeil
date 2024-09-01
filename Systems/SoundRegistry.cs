using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace LunarVeil.Systems
{
    internal static class SoundRegistry
    {
        public static string RootAssetPath => "LunarVeil/Assets/Sounds/";

        //Example how to add
        public static SoundStyle StormDragonLightningRain => new SoundStyle($"{RootAssetPath}StormDragon_LightingZap");
        public static SoundStyle BowCharge => new SoundStyle($"{RootAssetPath}BowCharge1");
        public static SoundStyle BowShoot => new SoundStyle($"{RootAssetPath}BowShot1");

    }
}
