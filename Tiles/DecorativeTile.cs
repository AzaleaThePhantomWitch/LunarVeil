using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LunarVeil.Tiles
{
    public abstract class DecorativeWallItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DecorativeWall decorativeWall = ModContent.GetModWall(Item.createWall) as DecorativeWall;
            if (decorativeWall == null)
                return false;
            decorativeWall.DrawItem(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
            return false;
        }
    }

    internal abstract class DecorativeWall : ModWall
    {
        public Color StructureColor { get; set; }
        public override string Texture => "LunarVeil/Tiles/InvisibleWall";
        public string StructureTexture { get; set; }
        public override void SetStaticDefaults()
        {
            StructureColor = Color.White;
            StructureTexture = this.GetType().FullName + "_S";
            StructureTexture = StructureTexture.Replace(".", "/");
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(200, 200, 200));
        }

        public void DrawItem(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(StructureTexture).Value;
            int textureWidth = texture.Width;
            int textureHeight = texture.Height;
            Vector2 drawOrigin = new Vector2(textureWidth / 2, textureHeight / 2);
            spriteBatch.Draw(texture, position, null, drawColor, 0, drawOrigin, scale * 0.5f, SpriteEffects.None, 0);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);
            Color color2 = Lighting.GetColor(i, j);
            Texture2D texture = ModContent.Request<Texture2D>(StructureTexture).Value;
            int textureWidth = texture.Width;
            int textureHeight = texture.Height;

            Vector2 drawPos = (new Vector2(i, j) + Systems.Tiling.MultitileHelper.TileAdj) * 16;
            Vector2 drawOrigin = new Vector2(textureWidth / 2, textureHeight);
            spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color2.MultiplyRGB(StructureColor), 0, drawOrigin, 1, SpriteEffects.None, 0);
        }
    }
}
