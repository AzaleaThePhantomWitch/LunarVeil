using LunarVeil.Systems;
using LunarVeil.Systems.Skies;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System.IO;
using System.Reflection;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.UI;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace LunarVeil
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class LunarVeil : Mod
    {
        public const string EMPTY_TEXTURE = "LunarVeil/Empty";
        public static Texture2D EmptyTexture
        {
            get;
            private set;
        }
        public int GlobalTimer { get; private set; }

     

     

        // this is alright, and i'll expand it so it can still be used, but really this shouldn't be used
        public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
        {
            packet.Write(msg);

            for (int m = 0; m < param.Length; m++)
            {
                object obj = param[m];
                if (obj is bool) packet.Write((bool)obj);
                else if (obj is byte) packet.Write((byte)obj);
                else if (obj is int) packet.Write((int)obj);
                else if (obj is float) packet.Write((float)obj);
                else if (obj is double) packet.Write((double)obj);
                else if (obj is short) packet.Write((short)obj);
                else if (obj is ushort) packet.Write((ushort)obj);
                else if (obj is sbyte) packet.Write((sbyte)obj);
                else if (obj is uint) packet.Write((uint)obj);
                else if (obj is decimal) packet.Write((decimal)obj);
                else if (obj is long) packet.Write((long)obj);
                else if (obj is string) packet.Write((string)obj);
            }
            return packet;
        }


        public static Player PlayerExists(int whoAmI)
        {
            return whoAmI > -1 && whoAmI < Main.maxPlayers && Main.player[whoAmI].active && !Main.player[whoAmI].dead && !Main.player[whoAmI].ghost ? Main.player[whoAmI] : null;
        }

       
        public LunarVeil()
        {
            Instance = this;
        }

        public static LunarVeil Instance;
        public override void Load()
        {
            LunarVeilUtils.LoadShaders();
            LunarVeilUtils.LoadOrderedLoadables();
            Instance = this;


            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            //``````````````````````````````````````````````````````````````````````````````````````


            //`````````````````````````````````````````````````````````````````````````````





            if (!Main.dedServ && Main.netMode != NetmodeID.Server && ModContent.GetInstance<LunarVeilClientConfig>().VanillaRespritesToggle == true)
            {
                Main.instance.LoadTiles(TileID.WoodBlock);
                TextureAssets.Tile[TileID.WoodBlock] = ModContent.Request<Texture2D>("LunarVeil/Tiles/VanillaRedo/ForestTiles/RefinedWoodTile");

                Main.instance.LoadTiles(TileID.Dirt);
                TextureAssets.Tile[TileID.Dirt] = ModContent.Request<Texture2D>("LunarVeil/Tiles/VanillaRedo/ForestTiles/CourseDirt");

                Main.instance.LoadTiles(TileID.Grass);
                TextureAssets.Tile[TileID.Grass] = ModContent.Request<Texture2D>("LunarVeil/Tiles/VanillaRedo/ForestTiles/CourseGrass");

                Main.instance.LoadTiles(TileID.ClayBlock);
                TextureAssets.Tile[TileID.ClayBlock] = ModContent.Request<Texture2D>("LunarVeil/Tiles/VanillaRedo/ForestTiles/ClayTile");

                //Set tree tops
                //Number inside is the one you're replacing, up to 31
                TextureAssets.TreeTop[0] = ModContent.Request<Texture2D>("LunarVeil/Tiles/VanillaRedo/ForestTiles/ClayTile");
            }


            On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) =>
            {
                orig(self, spriteBatch);
                DrawWorldSelectItemOverlay(self, spriteBatch);
            };


            Instance = this;
        }
        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            //    bool data = uiItem.Data.TryGetHeaderData(ModContent.GetInstance<WorldLoadGen>(), out var _data);
            UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
            WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiItem);
            WorldIcon.RemoveAllChildren();


            UIElement worldIcon = WorldIcon;
            UIImage element = new UIImage(ModContent.Request<Texture2D>("LunarVeil/Assets/Textures/Menu/LunarTree"))
            {
                Top = new StyleDimension(-10f, 0f),
                Left = new StyleDimension(-6f, 0f),
                IgnoresMouseInteraction = true
            };
            worldIcon.Append(element);


        }


        public override void Unload()
        {
            LunarVeilUtils.UnloadOrderedLoadables();

            if (!Main.dedServ)
            {
                UnloadTile(TileID.Grass);
                UnloadTile(TileID.Dirt);
                UnloadTile(TileID.WoodBlock);
            }
         }

        private void UnloadTile(int tileID)
        {
            TextureAssets.Tile[tileID] = ModContent.Request<Texture2D>($"Terraria/Images/Tiles_{tileID}");
        }

        private void UnloadWall(int wallID)
        {
            TextureAssets.Wall[wallID] = ModContent.Request<Texture2D>($"Terraria/Images/Wall_{wallID}");
        }

    }
}
