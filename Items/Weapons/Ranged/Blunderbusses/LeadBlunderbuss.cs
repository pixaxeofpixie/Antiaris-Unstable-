﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Antiaris.Items.Weapons.Ranged.Blunderbusses
{
    public class LeadBlunderbuss : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 13;
            item.ranged = true;
            item.width = 56;
            item.height = 18;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = 0;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 10;
            item.holdStyle = 3;
            item.shootSpeed = 12f;
            item.value = Item.sellPrice(0, 0, 8, 0);
            item.useAmmo = mod.ItemType("Buckshot");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lead Blunderbuss");
            Tooltip.SetDefault("Uses buckshots as ammo");
            DisplayName.AddTranslation(GameCulture.Chinese, "铅火铳");
            Tooltip.AddTranslation(GameCulture.Chinese, "使用火铳作为弹药");
            DisplayName.AddTranslation(GameCulture.Russian, "Свинцовый мушкетон");
            Tooltip.AddTranslation(GameCulture.Russian, "Использует картечь в качестве патронов");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BlunderbussBase", 1);
            recipe.AddIngredient(ItemID.LeadBar, 10);
            recipe.AddIngredient(ItemID.StoneBlock, 15);
            recipe.SetResult(this);
            recipe.AddTile(16);
            recipe.AddRecipe();
        }
    }
}
