using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.Map;
using Terraria.ModLoader;

namespace LunarVeil.WorldGeneration
{
    internal class LunarVeilWorldExtender : ModSystem
    {
        FieldInfo WorldGen_lastMaxTilesX;
        FieldInfo WorldGen_lastMaxTilesY;


        //We can set the world size to anything, 8400x2400 is large world
        public int NewMaxTilesX => 10400;
        public int NewMaxTilesY => 6400;
        //Original 8400x 2400y
        public override void Load()
        {
            //if (ModLoader.version < new Version(0, 10))
            //{
            //	throw new Exception("\nThis mod uses functionality only present in the latest tModLoader versions. Please update tModLoader to use this mod\n\n");
            //}
            IL_WorldGen.clearWorld += WorldGen_clearWorld;
            WorldGen_lastMaxTilesX = typeof(WorldGen).GetField("lastMaxTilesX", BindingFlags.Static | BindingFlags.NonPublic);
            WorldGen_lastMaxTilesY = typeof(WorldGen).GetField("lastMaxTilesY", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public override void Unload()
        {
            base.Unload();
            IL_WorldGen.clearWorld -= WorldGen_clearWorld;
        }
        private void WorldGen_clearWorld(ILContext il)
        {
            var cursor = new ILCursor(il);
            cursor.EmitDelegate<Action>(ClearWorld);
        }

        private void ClearWorld()
        {
            int lastMaxTilesX = (int)WorldGen_lastMaxTilesX.GetValue(null);
            int lastMaxTilesY = (int)WorldGen_lastMaxTilesY.GetValue(null);

            // TODO: investigate cpu/ram trade-off for reducing this later when regular-sized worlds loaded.
            if (Main.maxTilesX > 8400 && Main.maxTilesX > lastMaxTilesX || Main.maxTilesY > 2400 && Main.maxTilesY > lastMaxTilesY)
            {
                // Goal: Increase limits, don't decrease anything lower than normal max for compatibility.
                Main.maxTilesX = NewMaxTilesX;
                Main.maxTilesY = NewMaxTilesY;

                // TODO: dynamically change mapTargetX and Y to support any dimensions. (simple division.)
                // Map render targets. -- ingame map number of images to write to. The textures themselves
                Main.mapTargetX = 10; // change that 4 in vanilla to target-x
                Main.mapTargetY = 4; // change that 
                Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];

                int intendedMaxX = Math.Max(Main.maxTilesX + 1, 8401);
                int intendedMaxY = Math.Max(Main.maxTilesY + 1, 2401);

                // Individual map tiles
                Main.Map = new WorldMap(intendedMaxX, intendedMaxY);

                // Space for more tiles -- Actual tiles

                Tilemap tileMap = (Tilemap)typeof(Tilemap).GetConstructor(
                  BindingFlags.NonPublic | BindingFlags.Instance,
                  null, new Type[] { typeof(ushort), typeof(ushort) }, null).Invoke(new object[]
                  { (ushort)intendedMaxX, (ushort)intendedMaxY });
                Main.tile = tileMap;

                // Color for each tile

                Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
                Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
            }

        }
    }
}
