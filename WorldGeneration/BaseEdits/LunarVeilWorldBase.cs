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
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using LunarVeil.Systems.Tiling;
using LunarVeil.Tiles.RainforestTiles;

namespace LunarVeil.WorldGeneration.BaseEdits
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
                //Bring back an older pass
                if (tasks[i].Name == "Spawn Point")
                    continue;

                if (tasks[i].Name == "Quick Cleanup")
                    continue;
                
                if (tasks[i].Name == "Clean Up Dirt")
                    continue;

                if (tasks[i].Name == "Smooth World")
                    continue;

                if (tasks[i].Name == "Grass")
                    continue;

                if (tasks[i].Name == "Lakes")
                    continue;

                if (tasks[i].Name == "Spreading Grass")
                    continue;

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
      
        private void InsertNewPass(List<GenPass> tasks, string name, WorldGenLegacyMethod method, int index = -1)
        {
            if (index != -1)
            {
                tasks.Insert(index, new PassLegacy(name, method));
            }
            else
            {
                tasks.Add(new PassLegacy(name, method));
            }
        }


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

            /*
        //  RemovePass(tasks, "Spawn Point");
            RemovePass(tasks, "Full Desert");
            RemovePass(tasks, "Dirt Layer Caves");
            RemovePass(tasks, "Rock Layer Caves");
            RemovePass(tasks, "Surface Caves");
            RemovePass(tasks, "Wavy Caves");
            RemovePass(tasks, "Mountain Caves");
            RemovePass(tasks, "Generate Ice Biome");
            RemovePass(tasks, "Dungeon");
            RemovePass(tasks, "Corruption");
            RemovePass(tasks, "Floating Islands");
            RemovePass(tasks, "Buried Chests");
         // RemovePass(tasks, "Small Holes");
            RemovePass(tasks, "Jungle Temple");
            RemovePass(tasks, "Marble");
            RemovePass(tasks, "Granite");
            RemovePass(tasks, "Glowing Mushrooms and Jungle Plants");
            RemovePass(tasks, "Spider Caves");
            RemovePass(tasks, "Gem Caves");
            RemovePass(tasks, "Lihzahrd Altars");
            RemovePass(tasks, "Mushroom Patches");
            */



            //This completely replaces the vanilla pass

            // ReplacePassLegacy(tasks, "Terrain", NewSurfacing);

            //This function would remove every single pass except reset/terrain ones
            RemoveMostPasses(tasks);



            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Tunnels"));
            tasks[SurfaceLeveling] = new PassLegacy("LunarDiggin", (progress, config) =>
            {
                progress.Message = "Gintzia Digging Dunes in the ground and sands";
                NewCaveFormationMiddle(progress, config);
            });



            int Dune = tasks.FindIndex(genpass => genpass.Name.Equals("Dunes"));
            tasks[Dune] = new PassLegacy("LunarSands", (progress, config) =>
            {
                progress.Message = "Gintze eating sand";
                NewDunes(progress, config);
            });

            int RainforestingGen = tasks.FindIndex(genpass => genpass.Name.Equals("LunarSands"));
            if (RainforestingGen != -1)
            {
                tasks.Insert(RainforestingGen + 1, new PassLegacy("RainClump", RainforestClump));
                tasks.Insert(RainforestingGen + 2, new PassLegacy("RainDeeps", RainforestDeeps));
                tasks.Insert(RainforestingGen + 3, new PassLegacy("RainTrees", RainforestTreeSpawning));
            }


            int JungleGen = tasks.FindIndex(genpass => genpass.Name.Equals("RainDeeps"));
            if (JungleGen != -1)
            {
                tasks.Insert(JungleGen + 1, new PassLegacy("JungleClump", JungleClump));
                //tasks.Insert(JungleGen + 2, new PassLegacy("RainDeeps", RainforestDeeps));
            }

            int IceGen = tasks.FindIndex(genpass => genpass.Name.Equals("JungleClump"));
            if (IceGen != -1)
            {
                tasks.Insert(IceGen + 1, new PassLegacy("IceClump", IceClump));
                //tasks.Insert(JungleGen + 2, new PassLegacy("RainDeeps", RainforestDeeps));
            }

        }

        int desertNForest = 0;
        int jungleNIce = 0;
        int cinderNGovheilia = 0;
        int noxNDread = 0;


        #region  Dunes N Desert
       
        private void NewDunes(GenerationProgress progress, GameConfiguration configuration)
     {

            switch (Main.rand.Next(2))
            {
                case 0:
                    {

                        int attempts = 0;
                        while (attempts++ < 100)
                        {
                            // Select a place 
                            int smx = WorldGen.genRand.Next(((Main.maxTilesX) / 2) - 1300, (Main.maxTilesX / 2) - 550); // from 50 since there's a unaccessible area at the world's borders
                                                                                                                        // 50% of choosing the last 6th of the world
                                                                                                                        // Choose which side of the world to be on randomly
                            ///if (WorldGen.genRand.NextBool())
                            ///{
                            ///	towerX = Main.maxTilesX - towerX;
                            ///}

                            //Start at 200 tiles above the surface instead of 0, to exclude floating islands
                            int smy = (Main.maxTilesY / 4) - 200;

                            // We go down until we hit a solid tile or go under the world's surface
                           

                            while (!WorldGen.SolidTile(smx, smy) && smy <= Main.UnderworldLayer)
                            {
                                //seperation
                                smx += Main.rand.Next(21);
                                smy += 1;
                               
                            }

                            // If we went under the world's surface, try again
                            if (smy > Main.UnderworldLayer - 20)
                            {
                                
                                continue;
                            }

                            // If the type of the tile we are placing the tower on doesn't match what we want, try again

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy);
                                Point Loc3 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(70), Main.rand.Next(50) + 80), new Actions.SetTile(TileID.Sand));
                                WorldUtils.Gen(Loc3, new Shapes.Mound(200, 100), new Actions.SetTile(TileID.HardenedSand));


                                Point Loc4 = new Point(smx, smy - 50);
                                WorldGen.TileRunner(Loc4.X, Loc4.Y, 10, 4, TileID.HardenedSand, false, 0f, 0f, true, true);

                            }

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy + 100);
                                Point Loc3 = new Point(smx, smy + 20);

                                WorldUtils.Gen(Loc3, new Shapes.Mound(80, 100), new Actions.SetTile(TileID.HardenedSand));

                                WorldUtils.Gen(Loc2, new Shapes.Mound(60, 100), new Actions.SetTile(TileID.Sandstone));
                            }


                            for (int da = 0; da < 10; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 30, 0, 1, Main.rand.Next(100), 1, false);


                                WorldGen.digTunnel(smx, smy + 400, 0, 1, Main.rand.Next(100), 1, false);



                            }


                            for (int da = 0; da < 5; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);



                                Point Loc2 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(45), 100), new Actions.SetTile(TileID.HardenedSand));

                                Point Loc3 = new Point(smx, smy + 900);
                                WorldUtils.Gen(Loc3, new Shapes.Mound(Main.rand.Next(20), 700), new Actions.SetTile(TileID.HardenedSand));

                                Point Loc4 = new Point(smx + 12, smy + 900);
                                WorldUtils.Gen(Loc4, new Shapes.Mound(Main.rand.Next(10), 700), new Actions.SetTile(TileID.Sandstone));

                            }


                          



                            for (int da = 0; da < 20; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 100, 0, 1, 300, 5, false);



                                WorldGen.digTunnel(smx - Main.rand.Next(40), smy + Main.rand.Next(400) + 500, 0, 1, 100, 2, false);



                            }

                            for (int da = 0; da < 7; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);



                                Point Loc2 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(80) + 20, Main.rand.Next(200) + 50), new Actions.SetTile(TileID.Sandstone));



                            }

                            desertNForest = 1;
                        }

                    }

                   

                    break;



                case 1:
                    {

                        int attempts = 0;
                        while (attempts++ < 100)
                        {
                            // Select a place 
                            int smx = WorldGen.genRand.Next(((Main.maxTilesX) / 2) + 550, (Main.maxTilesX / 2) + 1300); // from 50 since there's a unaccessible area at the world's borders
                                                                                                                        // 50% of choosing the last 6th of the world
                                                                                                                        // Choose which side of the world to be on randomly
                            ///if (WorldGen.genRand.NextBool())
                            ///{
                            ///	towerX = Main.maxTilesX - towerX;
                            ///}

                            //Start at 200 tiles above the surface instead of 0, to exclude floating islands
                            int smy = (Main.maxTilesY / 4) - 200;

                            // We go down until we hit a solid tile or go under the world's surface
                           

                            while (!WorldGen.SolidTile(smx, smy) && smy <= Main.UnderworldLayer)
                            {
                                //seperation
                                smx += Main.rand.Next(21);
                                smy += 1;

                            }

                            // If we went under the world's surface, try again
                            if (smy > Main.UnderworldLayer - 20)
                            {
                                continue;
                            }

                            // If the type of the tile we are placing the tower on doesn't match what we want, try again

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy);
                                Point Loc3 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(70), Main.rand.Next(50) + 80), new Actions.SetTile(TileID.Sand));
                                WorldUtils.Gen(Loc3, new Shapes.Mound(200, 100), new Actions.SetTile(TileID.HardenedSand));


                                Point Loc4 = new Point(smx, smy - 50);
                                WorldGen.TileRunner(Loc4.X, Loc4.Y, 10, 4, TileID.HardenedSand, false, 0f, 0f, true, true);

                            }

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy + 100);
                                Point Loc3 = new Point(smx, smy + 20);

                                WorldUtils.Gen(Loc3, new Shapes.Mound(80, 100), new Actions.SetTile(TileID.HardenedSand));

                                WorldUtils.Gen(Loc2, new Shapes.Mound(60, 100), new Actions.SetTile(TileID.Sandstone));
                            }


                            for (int da = 0; da < 10; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 30, 0, 1, Main.rand.Next(100), 1, false);


                                WorldGen.digTunnel(smx, smy + 400, 0, 1, Main.rand.Next(100), 1, false);



                            }


                            for (int da = 0; da < 5; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);



                                Point Loc2 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(45), 100), new Actions.SetTile(TileID.HardenedSand));

                                Point Loc3 = new Point(smx, smy + 900);
                                WorldUtils.Gen(Loc3, new Shapes.Mound(Main.rand.Next(20), 700), new Actions.SetTile(TileID.HardenedSand));

                                Point Loc4 = new Point(smx + 12, smy + 900);
                                WorldUtils.Gen(Loc4, new Shapes.Mound(Main.rand.Next(10), 700), new Actions.SetTile(TileID.Sandstone));

                            }






                            for (int da = 0; da < 20; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 100, 0, 1, 300, 5, false);
                                WorldGen.digTunnel(smx - Main.rand.Next(40), smy + Main.rand.Next(400) + 500, 0, 1, 100, 2, false);


                            }

                            for (int da = 0; da < 7; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);



                                Point Loc2 = new Point(smx, smy + 400);
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(80) + 20, Main.rand.Next(200) + 50), new Actions.SetTile(TileID.Sandstone));



                            }


                            desertNForest = 2;
                        }

                    }


                    break;
            }



           

      }


        #endregion

        #region RainforestGeneration
        private void RainforestClump(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Forest Becoming Rainy";
            int smx = 0;
            int smy = 0;

            if (desertNForest == 2)
            {


                 smx = ((Main.maxTilesX) / 2) - 925;
                 smy = (Main.maxTilesY / 4) - 200;




                for (int da = 0; da < 1; da++)
                {
                    Point Loc7 = new Point(smx, smy);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y, 900, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 1200, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 1000, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                }


            }



            if (desertNForest == 1)
            {

                 smx = ((Main.maxTilesX) / 2) + 925;
                 smy = (Main.maxTilesY / 4) - 200;




                for (int da = 0; da < 1; da++)
                {
                    Point Loc7 = new Point(smx, smy);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y, 900, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 1200, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 1000, 2, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);
                }



            }




           







        }

        private void RainforestDeeps(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Forest Becoming deep";
          



            int attempts = 0;
            while (attempts++ < 1500)
            {
                int smx = WorldGen.genRand.Next(((Main.maxTilesX) / 4), ((Main.maxTilesX / 2) + (Main.maxTilesX) / 4));


                // Select a place // from 50 since there's a unaccessible area at the world's borders
                // 50% of choosing the last 6th of the world
                // Choose which side of the world to be on randomly
                ///if (WorldGen.genRand.NextBool())
                ///{
                ///	towerX = Main.maxTilesX - towerX;
                ///}

                //Start at 200 tiles above the surface instead of 0, to exclude floating islands
               int smy = (Main.maxTilesY / 3) - 700;

                // We go down until we hit a solid tile or go under the world's surface

                while (!WorldGen.SolidTile(smx, smy) && smy <= Main.UnderworldLayer)
                {
                    //seperation
                    smx += 15;
                    smy += 2;
                }

                // If we went under the world's surface, try again
                if (smy > Main.UnderworldLayer - 20)
                {
                    continue;
                }
                Tile tile = Main.tile[smx, smy];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (!(tile.TileType == ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>()))
                {
                    continue;
                }
                // If the type of the tile we are placing the tower on doesn't match what we want, try again


                for (int da = 0; da < 1; da++)
                {

                    //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);
                    //the true at the end makes it wet?
                    WorldGen.digTunnel(smx, smy, 0, 2, 125, 1, true);
                  
                    WorldGen.digTunnel(smx, smy + 150, 0, 2, 100, 2, true);

                    WorldGen.digTunnel(smx, smy + 300, 0, 2, 100, 3, true);

                    WorldGen.digTunnel(smx, smy + 500, 1, 2, 150, 2, true);

                    WorldGen.digTunnel(smx, smy + 700, 2, 2, 50, 2, true);

                    WorldGen.digTunnel(smx, smy + 750, 3, 2, 100, 1, true);

                    Point Loc7 = new Point(smx, smy + 150);
                    WorldGen.TileRunner(Loc7.X, Loc7.Y, 80, 4, ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>(), false, 0f, 0f, true, true);

                }


            }







        }

     

        private void RainforestTreeSpawning(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Making the Rainforest become a rainforest";
            for (int k = 60; k < Main.maxTilesX - 60; k++)
            {
                if (k > Main.maxTilesX / 3 && k < Main.maxTilesX / 3 * 2 && WorldGen.genRand.NextBool(1)) //inner part of the world
                {
                    for (int y = 10; y < Main.worldSurface; y++)
                    {
                        if (IsGround(k, y, 2))
                        {
                            PlaceRaintrees(k, y, Main.rand.Next(12, 60));
                            k += 1;

                            break;
                        }

                        if (!IsAir(k, y, 2))
                            break;
                    }
                }
            }


        }
        private bool IsAir(int x, int y, int w)
        {
            for (int k = 0; k < w; k++)
            {
                Tile tile = Framing.GetTileSafely(x + k, y);
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                    return false;
            }

            return true;
        }

        public static bool IsGround(int x, int y, int w)
        {
            for (int k = 0; k < w; k++)
            {
                Tile tile = Framing.GetTileSafely(x + k, y);
                if (!(tile.HasTile && tile.Slope == SlopeType.Solid && !tile.IsHalfBlock && (tile.TileType == ModContent.TileType<Tiles.RainforestTiles.RainforestGrass>())))
                    return false;

                Tile tile2 = Framing.GetTileSafely(x + k, y - 1);
                if (tile2.HasTile && Main.tileSolid[tile2.TileType])
                    return false;
            }

            return true;
        }

        public static void PlaceRaintrees(int treex, int treey, int height)
        {
            treey -= 1;

            if (treey - height < 1)
                return;

            for (int x = -1; x < 3; x++)
            {
                for (int y = 0; y < (height + 2); y++)
                {
                    WorldGen.KillTile(treex + x, treey - y);
                }
            }

            MultitileHelper.PlaceMultitile(new Point16(treex, treey - 1), ModContent.TileType<RainforestTreeBase>());

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    WorldGen.PlaceTile(treex + x, treey - (y + 2), ModContent.TileType<RainforestTree>(), true, true);
                }
            }

            for (int x = -1; x < 3; x++)
            {
                for (int y = 0; y < (height + 2); y++)
                {
                    WorldGen.TileFrame(treex + x, treey + y);
                }
            }
        }




        #endregion

        #region JungleGeneration
        private void JungleClump(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Jungle being goofy again";
            int smx = 0;
            int smy = 0;
            switch (Main.rand.Next(2))
            {
                case 0:
                    {


                        smx = ((Main.maxTilesX) / 2) - 1825;
                        smy = (Main.maxTilesY / 4) - 200;


                        jungleNIce = 1;

                        for (int da = 0; da < 1; da++)
                        {
                            Point Loc7 = new Point(smx, smy);
                            Point Loc8 = new Point(smx, smy + 50);
                            WorldUtils.Gen(Loc8, new Shapes.Mound(Main.rand.Next(80) + 450, Main.rand.Next(100) + 50), new Actions.SetTile(TileID.Mud));



                            WorldGen.TileRunner(Loc7.X, Loc7.Y, 1000, 6, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 800, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 700, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 900, 500, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1200, 300, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1500, 100, 2, TileID.Mud, false, 0f, 0f, true, true);
                        }



                        break;
                    }

                case 1:
                    {


                        smx = ((Main.maxTilesX) / 2) + 1825;
                        smy = (Main.maxTilesY / 4) - 200;

                        jungleNIce = 2;


                        for (int da = 0; da < 1; da++)
                        {
                            Point Loc7 = new Point(smx, smy);
                            Point Loc8 = new Point(smx, smy + 50);
                            WorldUtils.Gen(Loc8, new Shapes.Mound(Main.rand.Next(80) + 450, Main.rand.Next(100) + 50), new Actions.SetTile(TileID.Mud));

                            WorldGen.TileRunner(Loc7.X, Loc7.Y, 1000, 6, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 800, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 700, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 900, 500, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1200, 300, 2, TileID.Mud, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1500, 100, 2, TileID.Mud, false, 0f, 0f, true, true);
                        }



                        break;
                    }















            }
        }

        #endregion

        #region IceBiomeGeneration
        private void IceClump(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Ice biome mounding";
            int smx = 0;
            int smy = 0;
            bool placed = false;
          
                if (jungleNIce == 2)
                   
            {


                        smx = ((Main.maxTilesX) / 2) - 1825;
                        smy = (Main.maxTilesY / 4) - 200;


                  

                        for (int da = 0; da < 1; da++)
                        {
                            Point Loc7 = new Point(smx, smy);
                            Point Loc8 = new Point(smx, smy + 50);
                            WorldUtils.Gen(Loc8, new Shapes.Mound(450, 300), new Actions.SetTile(TileID.SnowBlock));


                            // Spawn in Ice Chunks
                            WorldGen.TileRunner(Loc7.X, Loc7.Y, 1000, 6, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 800, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 700, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 900, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1200, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1500, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1800, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1800, 700, 2, TileID.SnowBlock, false, 0f, 0f, true, true);



                    // Dig big chasm at top
                    WorldGen.digTunnel(smx, smy - 250, 0, 1, 1000, 15, false);

                    placed = true;

                }



                       
                }

                if (jungleNIce == 1)
                    {


                        smx = ((Main.maxTilesX) / 2) + 1825;
                        smy = (Main.maxTilesY / 4) - 200;

                   


                        for (int da = 0; da < 1; da++)
                        {
                            Point Loc7 = new Point(smx, smy);
                            Point Loc8 = new Point(smx, smy + 50);
                            WorldUtils.Gen(Loc8, new Shapes.Mound(450, 300), new Actions.SetTile(TileID.SnowBlock));



                            WorldGen.TileRunner(Loc7.X, Loc7.Y, 1000, 6, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 300, 800, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 600, 700, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 900, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1200, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1500, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1800, 500, 2, TileID.SnowBlock, false, 0f, 0f, true, true);
                            WorldGen.TileRunner(Loc7.X, Loc7.Y + 1800, 700, 2, TileID.SnowBlock, false, 0f, 0f, true, true);



                    // Dig big chasm at top
                    WorldGen.digTunnel(smx, smy - 250, 0, 1, 1000, 15, false);
                    placed = true;
                }



                        
                    }















            
        }
        #endregion




        #region CavesGeneration
        private void NewCaveFormationMiddle(GenerationProgress progress, GameConfiguration configuration)
        {
          


         
            int attempts = 0;
            while (attempts++ < 100000)
            {
                // Select a place 
                int smx = WorldGen.genRand.Next(((Main.maxTilesX) / 2) - 500, (Main.maxTilesX / 2) + 500); // from 50 since there's a unaccessible area at the world's borders
                                                                                                          // 50% of choosing the last 6th of the world
                                                                                                          // Choose which side of the world to be on randomly
                ///if (WorldGen.genRand.NextBool())
                ///{
                ///	towerX = Main.maxTilesX - towerX;
                ///}

                //Start at 200 tiles above the surface instead of 0, to exclude floating islands
                int smy = (Main.maxTilesY / 3) - 250;

                // We go down until we hit a solid tile or go under the world's surface
                Tile tile = Main.tile[smx, smy];

                while (!WorldGen.SolidTile(smx, smy) && smy <= Main.UnderworldLayer)
                {
                    //seperation
                    smx += 1;
                    smy += 30;
                    tile = Main.tile[smx, smy];
                }

                // If we went under the world's surface, try again
                if (smy > Main.UnderworldLayer - 20)
                {
                    continue;
                }

                // If the type of the tile we are placing the tower on doesn't match what we want, try again


                for (int da = 0; da < 1; da++)
                {
                    Point Loc = new Point(smx, smy + 350);
                    Point Loc2 = new Point(smx, smy + 100);
                    //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                    WorldGen.digTunnel(smx, smy, 2, 1, 10, 2, false);

                   // WorldGen.digTunnel(smx, smy - 300, 3, 1, 50, 2, true);




                }


            }

        }









#endregion

    }
}
