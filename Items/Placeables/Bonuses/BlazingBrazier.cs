﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Antiaris.Items.Placeables.Bonuses
{
    public class BlazingBrazier : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 3;
            item.createTile = mod.TileType("BlazingBrazier");
			//item.createTile = mod.TileType("Turret");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blazing Brazier");
            Tooltip.SetDefault("Increases movement speed for nearby players");
            DisplayName.AddTranslation(GameCulture.Chinese, "不灭篝火");
            Tooltip.AddTranslation(GameCulture.Chinese, "在其附近的玩家增加移动速度");
            DisplayName.AddTranslation(GameCulture.Russian, "Пылающая жаровня");
            Tooltip.AddTranslation(GameCulture.Russian, "Увеличивает скорость передвижения для ближайших игроков");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.anyIronBar = true;
            recipe.AddIngredient(null, "BlazingHeart");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}