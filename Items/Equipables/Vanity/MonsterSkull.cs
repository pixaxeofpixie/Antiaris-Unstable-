﻿using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Antiaris.Items.Equipables.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class MonsterSkull : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
			item.maxStack = 1;
			item.rare = 1;
            item.vanity = true;
			item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("MonsterSkull");
			item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monster Skull");
            DisplayName.AddTranslation(GameCulture.Chinese, "古生物骸骨");
            DisplayName.AddTranslation(GameCulture.Russian, "Череп монстра");
        }
    }
}
