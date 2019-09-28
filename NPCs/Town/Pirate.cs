﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Antiaris.NPCs.Town
{
    public class Pirate : ModNPC
    {
        public static bool Completed = false;
        private int timer = 0;

        public override void SetDefaults()
        {
            npc.dontTakeDamage = true;
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 26;
            npc.height = 50;
            npc.aiStyle = 7;
            npc.damage = 0;
            npc.defense = 50;
            npc.lifeMax = 700;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            animationType = -1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pirate");
            DisplayName.AddTranslation(GameCulture.Chinese, "船长");
            DisplayName.AddTranslation(GameCulture.Russian, "Пират");
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.64");
        }

        public override string TownNPCName()
        {
            return "Davy";
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            npc.life = npc.lifeMax;
        }

        public override void AI()
        {
            npc.rotation = 0f;
            var player = Main.player[Main.myPlayer];
            var pirateQuestSystem = player.GetModPlayer<PirateQuestSystem>(mod);
            npc.velocity.X = 0f;
            npc.velocity.Y = 5f;
            npc.direction = npc.spriteDirection;
            if ((double)player.position.X > (double)npc.position.X)
                npc.spriteDirection = 1;
            else if ((double)player.position.X < (double)npc.position.X)
                npc.spriteDirection = -1;
            if (Pirate.Completed)
            {
                npc.townNPC = false;
                timer++;
            }
            if (Main.netMode != 1 && npc.townNPC)
            {
                npc.homeless = false;
                npc.homeTileX = -1;
                npc.homeTileY = -1;
                npc.netUpdate = true;
            }
        }
    
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("Miscellaneous/QuestIcon2");
            Texture2D texture2 = mod.GetTexture("Miscellaneous/QuestIcon3");
            Player player = Main.player[Main.myPlayer];
            var pirateQuestSystem = player.GetModPlayer<PirateQuestSystem>();
            if (pirateQuestSystem.CurrentPirateQuest == -1 && !mod.GetModWorld<AntiarisWorld>().FinishedPirateQuest)
            {
                if (texture == null) return;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                float y = 50.0f;
                Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                spriteBatch.Draw(texture, position, null, Color.White, 0, origin, npc.scale, SpriteEffects.None, 0.0f);
            }
            foreach (var item in player.inventory)
            {
                if (item.type == mod.ItemType("MagicalAmulet") && pirateQuestSystem.CurrentPirateQuest != -1)
                {
                    if (texture == null) return;
                    Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    float y = 50.0f;
                    Vector2 position = npc.Center - Main.screenPosition - new Vector2(0.0f, y);
                    spriteBatch.Draw(texture2, position, null, Color.White, 0, origin, npc.scale, SpriteEffects.None, 0.0f);
                }
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                try
                {
                    PirateQuest();
                }
                catch (Exception exception)
                {
                    Main.NewText("Oh no, an error happened! Report this to Zerokk and send him the file Terraria/ModLoader/Logs/Logs.txt");
                    ErrorLogger.Log(exception);
                }
            }
        }

        void PirateQuest()
        {
            var pirateQuestSystem = Main.player[Main.myPlayer].GetModPlayer<PirateQuestSystem>(mod);
            if (Pirate.Completed)
            {
                Main.npcChatText = Language.GetTextValue("Mods.Antiaris.PirateCompleted");
                Main.npcChatCornerItem = 0;
            }
            else if (pirateQuestSystem.CurrentPirateQuest == -1)
            {
                var NewQuest = PirateQuestSystem.ChooseNewQuest();
                Main.npcChatText = PirateQuestSystem.Quests[NewQuest].ToString();
                Main.npcChatCornerItem = PirateQuestSystem.Quests[NewQuest].ItemType;
                pirateQuestSystem.CurrentPirateQuest = NewQuest;
                return;
            }
            else
            {
                if (pirateQuestSystem.CheckQuest())
                {
                    Main.npcChatText = pirateQuestSystem.GetCurrentQuest().SayThanks();
                    Main.npcChatCornerItem = 0;
                    pirateQuestSystem.SpawnReward(npc);
                    pirateQuestSystem.CompleteQuest(); ;
                    return;
                }
                else
                {
                    Main.npcChatText = pirateQuestSystem.GetCurrentQuest().ToString();
                    Main.npcChatCornerItem = pirateQuestSystem.GetCurrentQuest().ItemType;
                }
            }
        }

        public override string GetChat()
        {
            string PirateChat = Language.GetTextValue("Mods.Antiaris.PirateChat");
            return PirateChat;
        }

        public class PirateQuestSystem : ModPlayer
        {
            public static List<Quest> Quests = new List<Quest>();
            public int CurrentPirateQuest = -1;

            public override void Initialize()
            {
                Quests.Clear();
                Quests.Add(new Quest("Mods.Antiaris.PirateQuest", mod.ItemType("MagicalAmulet"), 1, 1d, "Mods.Antiaris.PirateThanks"));
            }

            public Quest GetCurrentQuest()
            {
                return Quests[CurrentPirateQuest];
            }

            public int currentPirateQuest
            {
                get { return CurrentPirateQuest; }
                set { CurrentPirateQuest = value; }
            }

            public override void clientClone(ModPlayer clientClone)
            {
                PirateQuestSystem clone = clientClone as PirateQuestSystem;
                clone.CurrentPirateQuest = CurrentPirateQuest;
            }

            public override void SendClientChanges(ModPlayer clientPlayer)
            {
                PirateQuestSystem clone = clientPlayer as PirateQuestSystem;
                if (clone.CurrentPirateQuest != CurrentPirateQuest)
                {
                    ModPacket packet = mod.GetPacket();
                    packet.Write((byte)4);
                    packet.Write(player.whoAmI);
                    packet.Write(CurrentPirateQuest);
                    packet.Send();
                }
            }

            public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)4);
                packet.Write(player.whoAmI);
                packet.Write(CurrentPirateQuest);
                packet.Send(toWho, fromWho);
            }

            public bool CheckQuest()
            {
                if (CurrentPirateQuest == -1)
                    return false;
                var quest = Quests[CurrentPirateQuest];
                if (player.CountItem(quest.ItemType, quest.ItemAmount) >= quest.ItemAmount)
                {
                    int LeftToRemove = quest.ItemAmount;
                    foreach (var item in player.inventory)
                    {
                        if (item.type == quest.ItemType)
                        {
                            int Removed = Math.Min(item.stack, LeftToRemove);
                            item.stack -= Removed;
                            LeftToRemove -= Removed;
                            if (item.stack <= 0)
                                item.SetDefaults();
                            if (LeftToRemove <= 0)
                                return true;
                        }
                    }
                }
                return false;
            }

            public void CompleteQuest()
            {
                var quest = Quests[CurrentPirateQuest];
                Pirate.Completed = true;
                CurrentPirateQuest = -1;
                mod.GetModWorld<AntiarisWorld>().FinishedPirateQuest = true;
            }

            public void SpawnReward(NPC npc)
            {
                switch (CurrentPirateQuest)
                {
                    case 0:
                        int number = 0;
                        number = Item.NewItem(npc.position, npc.Size, mod.ItemType("BrokenDavyKey"), 1, false, 0, false, false);
                        if (Main.netMode == 1 && number >= 0)
                            NetMessage.SendData(21, -1, -1, (NetworkText)null, number, 1f, 0.0f, 0.0f, 0, 0, 0);
                        return;
                }
            }

            public static int ChooseNewQuest()
            {
                return 0;
            }

            public override void Load(TagCompound tag)
            {
                CurrentPirateQuest = tag.GetInt("Current");
            }

            public override TagCompound Save()
            {
                var tag = new TagCompound();
                tag.Set("Current", CurrentPirateQuest);
                return tag;
            }
        }

        public class Quest
        {
            public int ItemAmount;
            public int ItemType;
            public string Name;
            public Action<NPC> SpawnReward;
            public string SpecialThanks;
            public double Weight;

            public Quest(string name, int itemID, int itemAmount = 1, double weight = 1d, string specialThanks = null, Action<NPC> spawnReward = null)
            {
                Name = name;
                ItemType = itemID;
                ItemAmount = itemAmount;
                Weight = weight;
                SpecialThanks = specialThanks;
                SpawnReward = spawnReward;
            }

            public override string ToString()
            {
                return Language.GetTextValue(Name, Main.LocalPlayer.name);
            }

            public string SayThanks()
            {
                return Language.GetTextValue(SpecialThanks);
            }
        }
    }
}
