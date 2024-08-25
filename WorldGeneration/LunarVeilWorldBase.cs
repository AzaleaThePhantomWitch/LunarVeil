using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.Localization;
using Terraria.IO;
using static tModPorter.ProgressUpdate;

namespace LunarVeil.WorldGeneration
{

    public class LunarVeilWorldBase : ModSystem
    {


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Terrain"));
            tasks[SurfaceLeveling] = new PassLegacy("Jungle Temple", (progress, config) =>
            {
                
                new PassLegacy("Surface Redesign", NewSurfacing);
        });







        }















        private void NewSurfacing(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Making a new surface";
            Main.worldSurface = Main.maxTilesY / 2;

        }
    }
}
