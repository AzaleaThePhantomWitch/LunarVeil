using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace LunarVeil.WorldGeneration
{
    internal class LunarVeilWorldExtender : ModSystem
    {
        public override void Load()
        {
            base.Load();
            IL_UIWorldCreation.FinishCreatingWorld += AddWorldSize;
        }

        public override void Unload()
        {
            base.Unload();
            IL_UIWorldCreation.FinishCreatingWorld -= AddWorldSize;
        }

        private void AddWorldSize(ILContext il)
        {
            var cursor = new ILCursor(il); 
            cursor.TryGotoNext(n => n.MatchCall(typeof(WorldGen), "CreateNewWorld"));
            cursor.Index -= 2;
            cursor.EmitDelegate<Action>(EditWSize);
        }

        private void EditWSize()
        {
            //8400x2400 is large world
            int tileWidth = 8400;
            int tileHeight = 2400;

            Main.maxTilesX = tileWidth;
            Main.maxTilesY = tileHeight;
        }
    }
}
