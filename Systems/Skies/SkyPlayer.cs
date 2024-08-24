using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;

namespace LunarVeil.Systems.Skies
{
    public class SkyPlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            base.PostUpdateMiscEffects();
            if (Main.netMode == NetmodeID.Server)
                return;

            if (!SkyManager.Instance["LunarVeil:CloudySky"].IsActive() 
                && Player.ZoneOverworldHeight || Player.ZoneSkyHeight || Player.ZoneUnderworldHeight)
            {
                Vector2 targetCenter = Player.Center;
                SkyManager.Instance.Activate("LunarVeil:CloudySky", targetCenter);
            }else if (SkyManager.Instance["LunarVeil:CloudySky"].IsActive())
            {
                SkyManager.Instance.Deactivate("LunarVeil:CloudySky");
            }

            if (!SkyManager.Instance["LunarVeil:DesertSky"].IsActive() && Player.ZoneDesert)
            {
                Vector2 targetCenter = Player.Center;
                SkyManager.Instance.Activate("LunarVeil:DesertSky", targetCenter);
            } else if (SkyManager.Instance["LunarVeil:DesertSky"].IsActive())
            {
                SkyManager.Instance.Deactivate("LunarVeil:DesertSky");
            }
        }
    }

}
