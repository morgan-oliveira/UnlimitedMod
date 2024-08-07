using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using UnlimitedMod.system.DiabloItem;
using Terraria.ModLoader.IO;

namespace UnlimitedMod.system.DiabloItem
{
    public class PrefixGenerator : GlobalItem
    {
        // Inicializando objeto do tipo PrefixController, que gerencia os dados desserializados do arquivo JSON.
        // Inicializando dicionário de ações por atributo, usando como chave os atributos, e como valor o delegate Action<>.
        private Dictionary<string, Action<Item>> attributeActions;
        public static List<PrefixController> prefixes = new List<PrefixController>();
        public static List<string> splitAtr;
        public static int Randomizer;
        public override bool InstancePerEntity => true;

        public bool visual = true;
        public override void Load()
        {
            string jsonFilePath = Path.Combine(ModLoader.ModPath, "Prefixes.JSON");
            if (File.Exists(jsonFilePath))
            {
                // Desserializando o JSON
                string jsonContent = File.ReadAllText(jsonFilePath);
                var rootJson = JsonConvert.DeserializeObject<Dictionary<string, List<PrefixController>>>(jsonContent);
                prefixes = rootJson["prefixes"];
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (DiabloItem.ValidateItem(item))
            {
                ApplyPrefixes(item);
            }
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (DiabloItem.ValidateItem(item))
            {
                
                ApplyPrefixes(item);
            }

        }

        public override void UpdateEquip(Item item, Player player)
        {
            // Manually adding prefix effect because I'm stupid
            if (item.Name.Contains("Arcane") || item.Name.Contains("Hex") || item.Name.Contains("Mana-loved"))
            {
                player.statManaMax2 += 20;
                player.statMana += 20;
            }
            if (item.Name.Contains("Insane"))
            {
                player.moveSpeed += 0.2f;
            }
            if (item.Name.Contains("Adventurous"))
            {
                player.jumpSpeedBoost += 0.3f;
                player.frogLegJumpBoost = true;
            }
        }
        // TODO: Finalizar funcionamento do método que associa um prefixo a um item
        public void ApplyPrefixes(Item item)
        {
            attributeActions = new Dictionary<string, Action<Item>> {
                { "enhdmg", DiabloItem.ApplyEnhancedDamage },
                { "knockback", DiabloItem.ApplyKnockback },
                { "FCR", DiabloItem.ApplyFCR },
                { "atkspd", DiabloItem.ApplyATKSPD },
                { "manaCost", DiabloItem.ApplyManaCost },
                { "movespd", DiabloItem.ApplyMoveSPD },
                { "jumpspd", DiabloItem.ApplyJumpSPD },
                { "critchance", DiabloItem.ApplyCritChance },
                { "poisonTag", DiabloItem.ApplyPoisonTag },
                { "poisondmg", DiabloItem.GeneratePoisonDamage },

            };

            if (prefixes != null && prefixes.Count > 0)
            {
                Randomizer = Main.rand.Next(0, prefixes.Count);
                item.GetGlobalItem<PrefixController>().Atr = prefixes[Randomizer].Atr;
                item.SetNameOverride($"{prefixes[Randomizer].PrefixName} {item.Name}");
                splitAtr = new List<string>(prefixes[Randomizer].Atr.Split(','));

                foreach (string atribute in splitAtr)
                {
                    // Chama o método associado ao atributo
                    attributeActions[atribute](item);
                }

            }
        }


        #region TagCompoundSaves
        public override void SaveData(Item item, TagCompound tag)
        {
            tag["itemUseTime"] = item.useTime;
            tag["itemUseAnimation"] = item.useAnimation;
            tag["itemMana"] = item.mana;
            tag["itemKnockback"] = item.knockBack;
            tag["itemCrit"] = item.crit;
            tag["itemName"] = item.Name;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            item.useTime = tag.GetInt("itemUseTime");
            item.useAnimation = tag.GetInt("itemUseAnimation");
            item.mana = tag.GetInt("itemMana");
            item.knockBack = tag.GetFloat("itemKnockback");
            item.crit = tag.GetInt("itemCrit");
            item.SetNameOverride(tag.GetString("itemName"));
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {

        }
        public override void NetReceive(Item item, BinaryReader reader)
        {

        }
        #endregion
    }
}
