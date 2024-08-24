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

     

        public ModPacket GetPacket(MessageType type, int capacity)
        {
            ModPacket packet = GetPacket(capacity + 1);
            packet.Write((byte)type);
            return packet;
        }

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

        public override void HandlePacket(BinaryReader reader, int whoAmI) => StellaMultiplayer.HandlePacket(reader, whoAmI);


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
        }

        public override void Unload()
        {
            LunarVeilUtils.UnloadOrderedLoadables();
        }
    }
}
