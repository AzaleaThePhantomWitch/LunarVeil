using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;

namespace LunarVeil.WorldGeneration
{

    public class LunarVeilWorldBase : ModSystem
    {

        
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Lakes"));








        }

    }
}
