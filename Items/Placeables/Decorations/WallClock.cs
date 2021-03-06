﻿using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Antiaris.Items.Placeables.Decorations
{
    public class WallClock : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.createTile = mod.TileType("WallClock");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wall Clock");
            DisplayName.AddTranslation(GameCulture.Chinese, "挂钟");
            DisplayName.AddTranslation(GameCulture.Russian, "Настенные часы");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Antiaris:GoldBar", 5);
            recipe.SetResult(this);
            recipe.AddTile(16);
            recipe.AddRecipe();
        }
    }
}
