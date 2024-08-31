using Terraria.Audio;

namespace LunarVeil.Systems
{
    internal static class SoundRegistry
    {
        public static string RootAssetPath => "LunarVeil/Assets/Sounds/";

        //Example how to add
        public static SoundStyle StormDragonLightningRain => new SoundStyle($"{RootAssetPath}StormDragon_LightingZap");
    }
}
