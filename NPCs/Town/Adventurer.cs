using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Antiaris.NPCs.Town
{
    [AutoloadHead]
    public class Adventurer : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "Antiaris/NPCs/Town/Adventurer";
            }
        }

        public override string[] AltTextures
        {
            get
            {
                return new string[] { "Antiaris/NPCs/Town/Adventurer2" };
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "Adventurer";
            return mod.Properties.Autoload;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 24;
            npc.height = 46;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            animationType = NPCID.GoblinTinkerer;
            NPCID.Sets.HatOffsetY[npc.type] = 10;
        }
		
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adventurer");
            DisplayName.AddTranslation(GameCulture.Chinese, "冒险家");
            DisplayName.AddTranslation(GameCulture.Russian, "Путешественник");
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 5;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 550;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 30;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.MagicAuraColor[npc.type] = new Color(195, 83, 119);
        }
		
        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(0, 11))
            {
                case 0:
                    return "Indiana";
                case 1:
                    return "Steve";
                case 2:
                    return "Marco";
                case 3:
                    return "Edmund";
                case 4:
                    return "Aron";
                case 5:
                    return "Tom";
                case 6:
                    return "Robert";
                case 7:
                    return "Ernest";
                case 8:
                    return "Charley";
                case 9:
                    return "Rolf";
                case 10:
                    return "David";
                default:
                    return "John";
            }
        }

        public override void TownNPCAttackMagic(ref float auraLightMultiplier)
        {
            auraLightMultiplier = 0.5f;
        }

        public override string GetChat()
        {
            Player player = Main.player[Main.myPlayer];
            string Adventurer1 = Language.GetTextValue("Mods.Antiaris.Adventurer1");
            string Adventurer2 = Language.GetTextValue("Mods.Antiaris.Adventurer2");
            string Adventurer3 = Language.GetTextValue("Mods.Antiaris.Adventurer3");
            string Adventurer4 = Language.GetTextValue("Mods.Antiaris.Adventurer4");
            string Adventurer5 = Language.GetTextValue("Mods.Antiaris.Adventurer5");
            string Adventurer6 = Language.GetTextValue("Mods.Antiaris.Adventurer6");
            string anglerName = "";
            int angler = NPC.FindFirstNPC(NPCID.Angler);
            if (angler >= 0)
            {
                anglerName = Main.npc[angler].GivenName;
            }
            string Adventurer7 = Language.GetTextValue("Mods.Antiaris.Adventurer7", anglerName);
            string Adventurer8 = Language.GetTextValue("Mods.Antiaris.Adventurer8");
            string AdventurerWerewolf = Language.GetTextValue("Mods.Antiaris.AdventurerWerewolf");
            if (angler >= 0 && Main.rand.Next(4) == 0)
            {
                return Adventurer7;
            }
            if (!Main.dayTime && Main.rand.Next(4) == 0)
            {
                return Adventurer8;
            }
            if (player.GetModPlayer<AntiarisPlayer>(mod).isWerewolf && Main.rand.Next(3) == 0)
            {
                return AdventurerWerewolf;
            }
            switch (Main.rand.Next(8))
            {
                case 0:
                    return Adventurer1;
                case 1:
                    return Adventurer2;
                case 2:
                    return Adventurer3;
                case 3:
                    return Adventurer4;
                case 4:
                    return Adventurer5;
                case 5:
                    return Adventurer6;
                case 6:
                    return Adventurer8;
                default:
                    return Adventurer4;
            }  
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Lang.inter[64].Value;
            button2 = Lang.inter[28].Value;
        }

        string NoQuest1 = Language.GetTextValue("Mods.Antiaris.NoQuest1");
        string NoQuest2 = Language.GetTextValue("Mods.Antiaris.NoQuest2");
        string NoQuest3 = Language.GetTextValue("Mods.Antiaris.NoQuest3");
        int ChangeState = 0;
        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            Main.npcChatCornerItem = 0;
            if (firstButton)
            {
                try
                {
                    DoThing();
                }
                catch (Exception exception)
                {
                    Main.NewText("Oh no, an error happened! Report this to Zerokk and send him the file Terraria/ModLoader/Logs/Logs.txt");
                    ErrorLogger.Log(exception);
                }
            }
            else
            {
                shop = true;
            }
        }

        private int getWeaponType()
        {
            if (Main.hardMode)
            { return 2; }
            else if (NPC.downedBoss1)
            { return 1; }
            return 0;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return mod.GetModWorld<AntiarisWorld>().savedAdventurer || (!NPC.AnyNPCs(mod.NPCType("Adventurer")) && !NPC.AnyNPCs(mod.NPCType("FrozenAdventurer")));
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            switch (getWeaponType())
            {
                case 2:
                    damage = 40;
                    knockback = 5f;
                    break;
                case 1:
                    damage = 21;
                    knockback = 5f;
                    break;
                default:
                    damage = 10;
                    knockback = 0f;
                    break;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            switch (getWeaponType())
            {
                case 2:
                    cooldown = 32;
                    randExtraCooldown = 33;
                    break;
                case 1:
                    cooldown = 36; 
                    randExtraCooldown = 26;
                    break;
                default:
                    cooldown = 40; 
                    randExtraCooldown = 20;
                    break;
            }
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            nightmare = true;
            switch (getWeaponType())
            {
                case 2:
                    projType = mod.ProjectileType("AdventurerAttack3");
                    attackDelay = 30; 
                    if (npc.localAI[3] == attackDelay - 1) Main.PlaySound(2, npc.Center, 43);
                    break;
                case 1:
                    projType = mod.ProjectileType("AdventurerAttack2");
                    attackDelay = 30;
                    if (npc.localAI[3] == attackDelay - 1) Main.PlaySound(2, npc.Center, 43);
                    break;
                default:
                    projType = mod.ProjectileType("AdventurerAttack1");
                    attackDelay = 30; 
                    if (npc.localAI[3] == attackDelay - 1) Main.PlaySound(2, npc.Center, 43);
                    break;
            }
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            gravityCorrection = 0f;
            switch (getWeaponType())
            {
                case 2:
                    multiplier = 13f;
                    randomOffset = 0.6f;
                    break;
                case 1:
                    multiplier = 9f;
                    randomOffset = 0.3f;
                    break;
                default:
                    multiplier = 9f;
                    randomOffset = 0.5f;
                    gravityCorrection = 1f;
                    break;
            }
        }

        private float timer;
        public override void AI()
        {
            ++timer;
            if ((double)timer % 75.0 == 0.0)
            {
                timer = 0f;
                nightmare = false;
            }
        }

        private bool nightmare;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var GlowMask = mod.GetTexture("Glow/Adventurer_GlowMask");
            var Effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(GlowMask, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
            Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, Effects, 0);
            Texture2D texture = mod.GetTexture("Miscellaneous/QuestIcon2");
            Texture2D texture2 = mod.GetTexture("Miscellaneous/QuestIcon3");
            Texture2D texture3 = mod.GetTexture("Miscellaneous/AdventurerNightmare");
            Player player = Main.player[Main.myPlayer];
            var questSystem = player.GetModPlayer<QuestSystem>();
            if (nightmare)
            {
                if (texture3 == null) return;
                Vector2 origin = new Vector2(texture3.Width / 2, texture3.Height / 2);
                float y = 50.0f;
                Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                spriteBatch.Draw(texture3, position, null, Color.White, 0, origin, npc.scale * 1.2f, SpriteEffects.None, 0.0f);
            }
            if (nightmare) return;
            if (questSystem.CurrentQuest == -1 && !questSystem.CompletedToday)
            {
                if (texture == null) return;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                float y = 50.0f;
                Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                spriteBatch.Draw(texture, position, null, Color.White, 0, origin, npc.scale, SpriteEffects.None, 0.0f);
            }
            if (questSystem.CurrentQuest >= 0 && questSystem.CurrentQuest != -1 && questSystem.GetCurrentQuest() is ItemQuest && player.CountItem((questSystem.GetCurrentQuest() as ItemQuest).ItemType, (questSystem.GetCurrentQuest() as ItemQuest).ItemAmount) >= (questSystem.GetCurrentQuest() as ItemQuest).ItemAmount)
            {
                int leftToRemove = (questSystem.GetCurrentQuest() as ItemQuest).ItemAmount;
                foreach (Item item in player.inventory)
                {
                    if (item.type == (questSystem.GetCurrentQuest() as ItemQuest).ItemType)
                    {
                        if (texture2 == null) return;
                        Vector2 origin = new Vector2(texture2.Width / 2, texture2.Height / 2);
                        float y = 50.0f;
                        Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                        spriteBatch.Draw(texture2, position, null, Color.White, 0, origin, npc.scale, SpriteEffects.None, 0.0f);
                    }
                }
            }
            if (questSystem.CurrentQuest >= 0 && questSystem.CurrentQuest != -1 && questSystem.GetCurrentQuest() is KillQuest && questSystem.QuestKills >= (questSystem.GetCurrentQuest() as KillQuest).TargetCount)
            {
                if (texture2 == null) return;
                Vector2 origin = new Vector2(texture2.Width / 2, texture2.Height / 2);
                float y = 50.0f;
                Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                spriteBatch.Draw(texture2, position, null, Color.White, 0, origin, npc.scale, SpriteEffects.None, 0.0f);
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            var questSystem = Main.player[Main.myPlayer].GetModPlayer<QuestSystem>(mod);		
            shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurerCrystal"));
            shop.item[nextSlot].shopCustomPrice = new int?(5);
            shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
            nextSlot++;
            if (questSystem.CompletedTotal >= 7)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurerSign"));
                shop.item[nextSlot].shopCustomPrice = new int?(15);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
            if (questSystem.CompletedTotal >= 17)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurerStar"));
                shop.item[nextSlot].shopCustomPrice = new int?(10);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
            if (questSystem.CompletedTotal >= 27)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurerSigil"));
                shop.item[nextSlot].shopCustomPrice = new int?(10);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
            if (questSystem.CompletedTotal >= 37)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurerEmblem"));
                shop.item[nextSlot].shopCustomPrice = new int?(5);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
			if (questSystem.CompletedTotal >= 12 && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("ArchaelogistManual"));
                shop.item[nextSlot].shopCustomPrice = new int?(15);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("CelestialManual"));
                shop.item[nextSlot].shopCustomPrice = new int?(5);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurersMachete"));
                shop.item[nextSlot].shopCustomPrice = new int?(12);
                shop.item[nextSlot].shopSpecialCurrency = Antiaris.coin;
                nextSlot++;
            }
        }

        void DoThing()
        {
            string Name1 = Language.GetTextValue("Mods.Antiaris.Name1");
            string Name10 = Language.GetTextValue("Mods.Antiaris.Name10");
            string Name11 = Language.GetTextValue("Mods.Antiaris.Name11");
            string Name12 = Language.GetTextValue("Mods.Antiaris.Name12");
            string Name13 = Language.GetTextValue("Mods.Antiaris.Name13");
            string Name14 = Language.GetTextValue("Mods.Antiaris.Name14");
            string Name15 = Language.GetTextValue("Mods.Antiaris.Name15");
            string Name16 = Language.GetTextValue("Mods.Antiaris.Name16");
            string Name17 = Language.GetTextValue("Mods.Antiaris.Name17");
            string Name18 = Language.GetTextValue("Mods.Antiaris.Name18");
            string Name19 = Language.GetTextValue("Mods.Antiaris.Name19");
            string Name2 = Language.GetTextValue("Mods.Antiaris.Name2");
            string Name20 = Language.GetTextValue("Mods.Antiaris.Name20");
            string Name21 = Language.GetTextValue("Mods.Antiaris.Name21");
            string Name3 = Language.GetTextValue("Mods.Antiaris.Name3");
            string Name4 = Language.GetTextValue("Mods.Antiaris.Name4");
            string Name5 = Language.GetTextValue("Mods.Antiaris.Name5");
            string Name6 = Language.GetTextValue("Mods.Antiaris.Name6");
            string Name7 = Language.GetTextValue("Mods.Antiaris.Name7");
            string Name8 = Language.GetTextValue("Mods.Antiaris.Name8");
            string Name9 = Language.GetTextValue("Mods.Antiaris.Name9");
            string Quest1 = Language.GetTextValue("Mods.Antiaris.Quest1");
            string Quest10 = Language.GetTextValue("Mods.Antiaris.Quest10");
            string Quest11 = Language.GetTextValue("Mods.Antiaris.Quest11");
            string Quest12 = Language.GetTextValue("Mods.Antiaris.Quest12");
            string Quest13 = Language.GetTextValue("Mods.Antiaris.Quest13");
            string Quest14 = Language.GetTextValue("Mods.Antiaris.Quest14");
            string Quest15 = Language.GetTextValue("Mods.Antiaris.Quest15");
            string Quest16 = Language.GetTextValue("Mods.Antiaris.Quest16");
            string Quest17 = Language.GetTextValue("Mods.Antiaris.Quest17");
            string Quest18 = Language.GetTextValue("Mods.Antiaris.Quest18");
            string Quest19 = Language.GetTextValue("Mods.Antiaris.Quest19");
            string Quest2 = Language.GetTextValue("Mods.Antiaris.Quest2");
            string Quest20 = Language.GetTextValue("Mods.Antiaris.Quest20");
            string Quest21 = Language.GetTextValue("Mods.Antiaris.Quest21");
            string Quest3 = Language.GetTextValue("Mods.Antiaris.Quest3");
            string Quest4 = Language.GetTextValue("Mods.Antiaris.Quest4");
            string Quest5 = Language.GetTextValue("Mods.Antiaris.Quest5");
            string Quest6 = Language.GetTextValue("Mods.Antiaris.Quest6");
            string Quest7 = Language.GetTextValue("Mods.Antiaris.Quest7");
            string Quest8 = Language.GetTextValue("Mods.Antiaris.Quest8");
            string Quest9 = Language.GetTextValue("Mods.Antiaris.Quest9");
            string NoQuest1 = Language.GetTextValue("Mods.Antiaris.NoQuest1");
            string NoQuest2 = Language.GetTextValue("Mods.Antiaris.NoQuest2");
            string NoQuest3 = Language.GetTextValue("Mods.Antiaris.NoQuest3");
            string ThanksRod2 = Language.GetTextValue("Mods.Antiaris.ThanksRod2");
            foreach (Player player in Main.player)
            {
                if (player.active && player.talkNPC == npc.whoAmI)
                {
                    var questSystem = player.GetModPlayer<QuestSystem>(mod);
					if (QuestSystem.BrokenRod && questSystem.CurrentQuest != 11)
					{
						QuestSystem.BrokenRod = false;
					}
                    if (questSystem.CompletedToday)
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Main.npcChatText = NoQuest1; return;
                            case 1:
                                Main.npcChatText = NoQuest2; return;
                            default:
                                Main.npcChatText = NoQuest3; return;
                        }
                    }
                    else if (questSystem.CurrentQuest == -1)
                    {
                        int NewQuest = QuestSystem.ChooseNewQuest();
                        Main.npcChatText = QuestSystem.Quests[NewQuest].ToString();
                        if (QuestSystem.Quests[NewQuest] is ItemQuest)
                        {
                            Main.npcChatCornerItem = (QuestSystem.Quests[NewQuest] as ItemQuest).ItemType;
                            questSystem.CurrentQuest = NewQuest;
                        }
                        if (QuestSystem.Quests[NewQuest] is KillQuest)
                        {
                            Main.npcChatCornerItem = 0;
                            questSystem.CurrentQuest = NewQuest;
                        }
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (questSystem.CheckQuest())
                            {
                                if (!QuestSystem.BrokenRod)
                                {
                                    Main.npcChatText = questSystem.GetCurrentQuest().SayThanks();
                                }
                                else if (QuestSystem.BrokenRod && questSystem.CurrentQuest == 11)
                                {
                                    Main.npcChatText = ThanksRod2;
                                }
                                Main.PlaySound(12, -1, -1, 1);
                                questSystem.SpawnReward(npc);
                                questSystem.CompleteQuest();
                                return;
                            }
                            else
                            {
                                Main.npcChatText = questSystem.GetCurrentQuest().ToString();
                                if (questSystem.GetCurrentQuest() is ItemQuest)
                                {
                                    Main.npcChatCornerItem = (questSystem.GetCurrentQuest() as ItemQuest).ItemType;
                                }
                                if (questSystem.GetCurrentQuest() is KillQuest)
                                {
                                    string QuestKilled = Language.GetTextValue("Mods.Antiaris.QuestKilled");
                                    string QuestKilled2 = Language.GetTextValue("Mods.Antiaris.QuestKilled2");
                                    Main.npcChatText += QuestKilled + questSystem.QuestKills + QuestKilled2 + (questSystem.GetCurrentQuest() as KillQuest).TargetCount;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Main.NewText("Oh no, an error happened! Report this to Zerokk and send him the file Terraria/ModLoader/Logs/Logs.txt");
                            ErrorLogger.Log(exception);
                        }
                    }
                }
            }
        }

		public override void NPCLoot()
		{
			mod.GetModWorld<AntiarisWorld>().savedAdventurer = true;
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 151, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AdventurerGore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AdventurerGore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AdventurerGore3"), 1f);
                if (Main.netMode != 1)
                {
                    int centerX = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
                    int centerY = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                    int halfLength = npc.width / 2 / 16 + 1;
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AdventurerHat"), 1, false, 0, false, false);
                }
            }
        }
    }
}

