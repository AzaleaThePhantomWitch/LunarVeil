﻿using Terraria.Audio;

namespace LunarVeil.Systems
{
    internal static class SoundRegistry
    {
        public static string RootAssetPath => "LunarVeil/Assets/Sounds/";

        //Example how to add
        public static SoundStyle StormDragonLightningRain => new SoundStyle($"{RootAssetPath}StormDragon_LightingZap");
        public static SoundStyle BowCharge => new SoundStyle($"{RootAssetPath}BowCharge",
            variantSuffixesStart: 1,
            numVariants: 2);
        public static SoundStyle BowShoot => new SoundStyle($"{RootAssetPath}BowShot", 
            variantSuffixesStart: 1, 
            numVariants: 2);
    }
}
