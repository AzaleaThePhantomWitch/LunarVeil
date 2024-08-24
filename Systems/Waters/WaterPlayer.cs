using Terraria;
using Terraria.ModLoader;

namespace LunarVeil.Systems.Waters
{
    class WaterPlayer : ModPlayer
    {
        public delegate void PostUpdateDelegate(Player player);
        public static event PostUpdateDelegate PostUpdateEvent;
        public override void PostUpdate()
        {
            base.PostUpdate();
            PostUpdateEvent.Invoke(Player);
        }
    }
}
