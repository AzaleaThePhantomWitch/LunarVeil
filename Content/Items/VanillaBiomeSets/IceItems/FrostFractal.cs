using LunarVeil.Content.Bases;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LunarVeil.Content.Items.VanillaBiomeSets.IceItems
{
    public abstract class ModBow<HeldProjectile> : ModItem where HeldProjectile : HeldBow
    {
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<HeldProjectile>()] <= 0;
        public override bool CanConsumeAmmo(Item ammo, Player player) => false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Test Held Bow");
        }

        public virtual void SafeSetDefaults() { }
        public sealed override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HeldProjectile>();
            Item.noUseGraphic = true;
            Item.useAmmo = AmmoID.Arrow;

            SafeSetDefaults();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Item.shoot;
        }
    }

  

    public class FrostFractal : ModBow<FrostFractal_Held>
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Gamer Bow");
            // Tooltip.SetDefault("'THIS IS A TESTING ITEM'");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 6;
            Item.width = 32;
            Item.height = 74;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.rare = ItemRarityID.Lime;
            Item.useTime = 22;
            Item.useAnimation = 22;
        }
    }

    public class FrostFractal_Held : HeldBow
    {
        // You can add whatever you want here like a regular weapon
        public override Color StringColor => new Color(45, 35, 65);
        public override int ParentItem => ModContent.ItemType<FrostFractal>();
    }
}