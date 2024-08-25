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

            // tasks[SurfaceLeveling] = new PassLegacy("Surface Redo", (progress, config) =>
            //  {

            //      new PassLegacy("Surface Redesign", NewSurfacing);
            //   });
            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
            if (SurfaceLeveling != -1)
            {

                //tasks.Insert(CathedralGen2 + 1, new PassLegacy("World Gen Virulent Structures", WorldGenVirulentStructures));
                //	tasks.Insert(CathedralGen2 + 1, new PassLegacy("World Gen Virulent", WorldGenVirulent));

                tasks.Insert(SurfaceLeveling + 1, new PassLegacy("Lunar Veil Resurface", NewSurfacing));






            }

        }
        private void NewSurfacing(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Making a new surface level";
            Main.worldSurface = Main.maxTilesY / 2;

        }
    }
}
