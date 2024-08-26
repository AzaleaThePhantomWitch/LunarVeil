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

namespace LunarVeil.WorldGeneration.BaseEdits
{

    public class LunarVeilWorldBase : ModSystem
    {


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {

            int SurfaceLeveling = tasks.FindIndex(genpass => genpass.Name.Equals("Tunnels"));
            tasks[SurfaceLeveling] = new PassLegacy("LunarDiggin", (progress, config) =>
            {
                progress.Message = "Gintzia Digging Dunes in the ground and sands";
                NewCaveFormationMiddle(progress, config);
            });



            int Dune = tasks.FindIndex(genpass => genpass.Name.Equals("Dunes"));
            tasks[Dune] = new PassLegacy("LunarSands", (progress, config) =>
            {
                progress.Message = "More Sand";
                NewDunes(progress, config);
            });

        }
        int desertNForest = 0;
        int jungleNIce = 0;
        int cinderNGovheilia = 0;
        int noxNDread = 0;
       
        
        
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
                                smy -= 0;
                               
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
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(70), Main.rand.Next(150) + 50), new Actions.SetTile(TileID.Sand));
                                WorldUtils.Gen(Loc3, new Shapes.Mound(200, 100), new Actions.SetTile(TileID.HardenedSand));


                            }

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy + 100);
                                Point Loc3 = new Point(smx, smy + 20);

                                WorldUtils.Gen(Loc3, new Shapes.Mound(80, 100), new Actions.SetTile(TileID.HardenedSand));

                                WorldUtils.Gen(Loc2, new Shapes.Mound(60, 100), new Actions.SetTile(TileID.Sandstone));
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


                            for (int da = 0; da < 10; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 30, 0, 1, Main.rand.Next(100), 1, false);


                                WorldGen.digTunnel(smx, smy + 400, 0, 1, Main.rand.Next(100), 1, false);



                            }



                            for (int da = 0; da < 20; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 100, 0, 1, 400, 5, false);






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
                                smy -= 0;
                                
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
                                WorldUtils.Gen(Loc2, new Shapes.Mound(Main.rand.Next(70), Main.rand.Next(150) + 50), new Actions.SetTile(TileID.Sand));
                                WorldUtils.Gen(Loc3, new Shapes.Mound(200, 100), new Actions.SetTile(TileID.HardenedSand));


                            }

                            for (int daa = 0; daa < 1; daa++)
                            {
                                Point Loc2 = new Point(smx, smy + 100);
                                Point Loc3 = new Point(smx, smy + 20);

                                WorldUtils.Gen(Loc3, new Shapes.Mound(80, 100), new Actions.SetTile(TileID.HardenedSand));

                                WorldUtils.Gen(Loc2, new Shapes.Mound(60, 100), new Actions.SetTile(TileID.Sandstone));
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


                            for (int da = 0; da < 10; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 30, 0, 1, Main.rand.Next(100), 1, false);


                                WorldGen.digTunnel(smx, smy + 400, 0, 1, Main.rand.Next(100), 1, false);



                            }



                            for (int da = 0; da < 20; da++)
                            {
                                //StructureLoader.ReadStruct(Loc, "Struct/Underground/Manor", tileBlend);

                                WorldGen.digTunnel(smx, smy + 100, 0, 1, 400, 5, false);






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





                    
                }


            }

        }

    }
}
