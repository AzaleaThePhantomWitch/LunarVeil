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
using Terraria.ID;
using Terraria.GameContent.Biomes;
using Microsoft.Xna.Framework;

namespace LunarVeil.WorldGeneration
{

    public class LunarVeilWorldBase : ModSystem
    {
        private void DoNothing(GenerationProgress progress, GameConfiguration configuration) { }
        private void ReplacePassLegacy(List<GenPass> tasks, string name, WorldGenLegacyMethod method)
        {
            var pass = new PassLegacy(name, method);
            int index = tasks.FindIndex(genpass => genpass.Name.Equals(name));
            if (index != -1)
            {
                tasks[index] = pass;
            }
        }

        private void RemoveMostPasses(List<GenPass> tasks)
        {
            for (int i = 2; i < tasks.Count; i++)
            {
                tasks[i] = new PassLegacy(tasks[i].Name, DoNothing);
            }
        }

        private void RemovePass(List<GenPass> tasks, string name)
        {
            int caveIndex = tasks.FindIndex(genpass => genpass.Name.Equals(name));
            if (caveIndex != -1)
            {
                tasks[caveIndex] = new PassLegacy(name, DoNothing);
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

            // tasks[SurfaceLeveling] = new PassLegacy("Surface Redo", (progress, config) =>
            //  {

            //      new PassLegacy("Surface Redesign", NewSurfacing);
            //   });
            
            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Terrain"));
            if (SurfaceLeveling != -1)
            {
                tasks.Insert(SurfaceLeveling + 1, new PassLegacy("Lunar Veil Resurface", NewSurfacing));
            }

            //This completely replaces the vanilla pass
            ReplacePassLegacy(tasks, "Terrain", NewSurfacing);
          
            //This function would remove every single pass except reset/terrain ones
            //RemoveMostPasses(tasks);

            //These just remove a specific pass
            /*
            RemovePass(tasks, "Spawn Point");
            RemovePass(tasks, "Full Desert");
            RemovePass(tasks, "Dirt Layer Caves");
            RemovePass(tasks, "Rock Layer Caves");
            RemovePass(tasks, "Surface Caves");
            RemovePass(tasks, "Wavy Caves");
            RemovePass(tasks, "Mountain Caves");
            RemovePass(tasks, "Generate Ice Biome");
            RemovePass(tasks, "Dungeon");
            */
        }


        private void NewSurfacing(GenerationProgress progress, GameConfiguration configuration)
        {

        }
    }
}
