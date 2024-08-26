﻿using LunarVeil.WorldGeneration.BaseEdits;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace LunarVeil.Tiles.RainforestTiles
{
    internal class RainforestTreeSapling : ModTile
    {
       

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.RandomStyleRange = 3;
            TileObjectData.newTile.StyleHorizontal = true;

            var bottomAnchor = new AnchorData(Terraria.Enums.AnchorType.SolidTile, 1, 0);

            Main.tileTable[Type] = true;
            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

            AdjTiles = new int[] { TileID.Bookcases };
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;

            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
            TileObjectData.newTile.StyleMultiplier = 2; //same as above
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();

            // name.SetDefault("Alcaology Station");
            AddMapEntry(new Color(126, 200, 59), name);
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            if (!(tile.TileFrameX % 36 == 0 && tile.TileFrameY == 0))
                return;

            // vanilla tree seems to be about >5%?
            // at 3% after over an hour and a half 6/10 grew
            if (WorldGen.genRand.NextBool(10))
            { // still needs testing but should be fast enough
                return;
            }
            if (!LunarVeilWorldBase.IsGround(i - 1, j + 3, 4))
                return;

            int height = Main.rand.Next(10, 75);
            const int AbsMinHeight = 9; // lower than the min height normally

            for (int h = 0; h < height; h++) // finds clear area
            {
                if (!IsAir(i - 1, j + 2 - h, 4))
                {
                    if (h > AbsMinHeight) // if above min height, just use current height
                    {
                        height = h - 4; // needs to account for the height of the base
                        break;
                    }
                    else // if below min height, cancel
                    {
                        return;
                    }
                }
            }

            // removes sapling
            for (int g = 0; g < 2; g++)
            {
                for (int h = 0; h < 3; h++)
                    Main.tile[i + g, j + h].ClearTile();
            }

            LunarVeilWorldBase.PlaceRaintrees(i, j + 3, height);
        }

        private bool IsAir(int x, int y, int w) // method from worldgen, but needs to skip sapling and platform
        {
            for (int k = 0; k < w; k++)
            {
                Tile tile = Framing.GetTileSafely(x + k, y);
                if (tile.HasTile && tile.TileType != Type && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]) // this version allows the tree to break stuff the player can stand on but pass though (platforms, tool stations)
                    return false;
            }

            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }
    }


        internal class RainforestTree : ModTile
        {
           

            public override void SetStaticDefaults()
            {
                 LocalizedText name = CreateMapEntryName();
                  TileID.Sets.IsATreeTrunk[Type] = true;
                Main.tileAxe[Type] = true;
                AddMapEntry(new Color(169, 200, 93), name);

                RegisterItemDrop(ItemID.Wood);
            }

            private float GetLeafSway(float offset, float magnitude, float speed)
            {
                return (float)Math.Sin(Main.GameUpdateCount * speed + offset) * magnitude;
            }

            public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
            {
                bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();
                bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTree>();

                if (right && !up && down || !up && !down)
                    Main.instance.TilesRenderer.AddSpecialLegacyPoint(new Point(i, j));
            }

            public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
            {
                bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();
                bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTree>();

                if (right && !up && down)
                {
                    Texture2D tex = ModContent.Request<Texture2D>(Texture + "Top").Value;
                    Vector2 pos = (new Vector2(i + 1, j) + Systems.Tiling.MultitileHelper.TileAdj) * 16;

                    Color color = Lighting.GetColor(i, j);

                    spriteBatch.Draw(tex, pos - Main.screenPosition, null, color, GetLeafSway(3, 0.05f, 0.008f), new Vector2(tex.Width / 2, tex.Height), 1, 0, 0);



                    

                   
                }

                if (!up && !down)
                {
                    Texture2D sideTex = Terraria.GameContent.TextureAssets.TreeTop[0].Value;
                    Vector2 sidePos = (new Vector2(i + 1, j) + Systems.Tiling.MultitileHelper.TileAdj) * 16;

                    if (left)
                        spriteBatch.Draw(sideTex, sidePos + new Vector2(20, 0) - Main.screenPosition, null, Color.White, 0, Vector2.Zero, 1, 0, 0);

                    if (right)
                        spriteBatch.Draw(sideTex, sidePos + new Vector2(0, 20) - Main.screenPosition, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                }
            }

            public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
            {
                bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();
                bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTree>();

                if (right && !up && down)
                {
                    Texture2D tex = ModContent.Request<Texture2D>(Texture + "Top").Value;
                    Vector2 pos = (new Vector2(i + 1, j) + Systems.Tiling.MultitileHelper.TileAdj) * 16;

                    Color color = Lighting.GetColor(i, j);

                    spriteBatch.Draw(tex, pos + new Vector2(50, 40) - Main.screenPosition, null, color.MultiplyRGB(Color.Gray), GetLeafSway(0, 0.05f, 0.01f), new Vector2(tex.Width / 2, tex.Height), 1, 0, 0);
                    spriteBatch.Draw(tex, pos + new Vector2(-30, 80) - Main.screenPosition, null, color.MultiplyRGB(Color.DarkGray), GetLeafSway(2, 0.025f, 0.012f), new Vector2(tex.Width / 2, tex.Height), 1, 0, 0);
                }

                return true;
            }

          

            public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
            {
                if (fail || effectOnly)
                    return;

                Framing.GetTileSafely(i, j).HasTile = false;

                bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();
                bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTree>() ||
                    Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTreeBase>();

                if (left)
                    WorldGen.KillTile(i - 1, j);
                if (right)
                    WorldGen.KillTile(i + 1, j);
                if (up)
                    WorldGen.KillTile(i, j - 1);
                if (down)
                    WorldGen.KillTile(i, j + 1);
            }

            public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
            {
                short x = 0;
                short y = 0;

                bool left = Framing.GetTileSafely(i - 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool right = Framing.GetTileSafely(i + 1, j).TileType == ModContent.TileType<RainforestTree>();
                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();
                bool down = Framing.GetTileSafely(i, j + 1).TileType == ModContent.TileType<RainforestTree>();

                if (up || down)
                {
                    if (right)
                        x = 0;

                    if (left)
                        x = 18;

                    y = (short)(Main.rand.Next(3) * 18);

                    if (Main.rand.NextBool(3))
                        x += 18 * 2;
                }

                Tile tile = Framing.GetTileSafely(i, j);
                tile.TileFrameX = x;
                tile.TileFrameY = y;

                return false;
            }
        }
  
        class RainforestTreeBase : ModTile
        {
         

            public override void SetStaticDefaults()
            {
                TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
                Main.tileAxe[Type] = true;
                TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
                TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
          //  TileObjectData.newTile.Origin = new Point16(TileObjectData.newTile.Width / 2, TileObjectData.newTile.Height - 1);

            for (int k = 0; k < TileObjectData.newTile.Height; k++)
            {
                TileObjectData.newTile.CoordinateHeights[k] = 16;
            }

             //this breaks for some tiles: the two leads are multitiles and tiles with random styles
             TileObjectData.newTile.CoordinateHeights[TileObjectData.newTile.Height - 1] = 18;

            Main.tileSolidTop[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;// This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 2;

            TileObjectData.newTile.DrawXOffset = 16;

            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
        }

            public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
            {
                if (fail || effectOnly)
                    return;

                Framing.GetTileSafely(i, j).HasTile = false;

                bool up = Framing.GetTileSafely(i, j - 1).TileType == ModContent.TileType<RainforestTree>();

                if (up)
                    WorldGen.KillTile(i, j - 1);
            }
        }
    

    public class RainforestTreeAcorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Super silk!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;

        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.value = Item.buyPrice(silver: 50);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<RainforestTreeSapling>();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
     
            public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RainforestGrassBlock>(), 2);
            recipe.AddIngredient(ItemID.Acorn, 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
    
}