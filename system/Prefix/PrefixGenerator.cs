using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using UnlimitedMod.system.DiabloItem;

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
            if (ValidateItem(item))
            {
                RollInitialDamage(item);
                ApplyPrefixes(item);
            }
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (ValidateItem(item))
            {
                RollInitialDamage(item);
                ApplyPrefixes(item);
            }

        }

        public override void UpdateEquip(Item item, Player player)
        {
            // Manually adding prefix effect because I'm stupid
            if (item.Name.Contains("Arcane") || item.Name.Contains("Hex") || item.Name.Contains("Mana-loved")) {
                player.statManaMax2 += 20;
                player.statMana += 20;
            }
            if (item.Name.Contains("Insane")) {
                player.moveSpeed += 0.2f;
            }
            if (item.Name.Contains("Adventurous")) {
                player.jumpSpeedBoost += 0.3f;
                player.frogLegJumpBoost = true;
            }
        }
        // TODO: Finalizar funcionamento do método que associa um prefixo a um item
        public void ApplyPrefixes(Item item)
        {
            attributeActions = new Dictionary<string, Action<Item>> {
                { "mana", ApplyMana },
                { "enhdmg", ApplyEnhancedDamage },
                { "knockback", ApplyKnockback },
                { "FCR", ApplyFCR },
                { "atkspd", ApplyATKSPD },
                { "manaCost", ApplyManaCost },
                { "movespd", ApplyMoveSPD },
                { "jumpspd", ApplyJumpSPD },
                { "critchance", ApplyCritChance },
                { "poisonTag", ApplyPoisonTag }
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
        private bool ValidateItem(Item item) {
            if (item.damage > 0 || item.defense > 0 || item.accessory) {
                return true;
            } else return false;
        }

        private void ApplyPoisonTag(Item item)
        {
            item.GetGlobalItem<DiabloItem>().PoisonTag = true;
            item.GetGlobalItem<DiabloItem>().LightningTag = false;
            item.GetGlobalItem<DiabloItem>().FireTag = false;
            item.GetGlobalItem<DiabloItem>().ColdTag = false;
        }

        private void ApplyCritChance(Item item)
        {
            item.crit += 10;
        }

        private void ApplyJumpSPD(Item item)
        {
            //throw new NotImplementedException();
            Main.NewText("Tais pulandinho hein dog");
        }

        private void ApplyEnhancedDamage(Item item)
        {
            item.GetGlobalItem<DiabloItem>().EnhancedDamage = RollSystem.GenerateRoll(item, item.damage);
        }
        private void RollInitialDamage(Item item)
        {
            item.damage = (int)Math.Round(RollSystem.GenerateRoll(item, item.OriginalDamage));
        }

        private void ApplyMoveSPD(Item item)
        {
            Main.NewText("Tais voando dog");
            Player player = new Player();
            player.moveSpeed += 0.1f;
        }

        private void ApplyManaCost(Item item)
        {
            if (item.mana > 0)
            {
                item.mana -= (int)(item.mana * 0.5f);
            }
        }

        private void ApplyATKSPD(Item item)
        {
            if (item.DamageType == DamageClass.Melee)
            {
                item.useTime -= 7;
                item.useAnimation -= 7;
            }
        }

        private void ApplyFCR(Item item)
        {
            if (item.DamageType == DamageClass.Magic)
            {
                item.useTime -= 5;
                item.useAnimation -= 5;
            }
        }

        private void ApplyKnockback(Item item)
        {
            item.knockBack += 3.5f;
        }

        private void ApplyMana(Item item)
        {
            if (item.mana > 0)
            {
                Main.LocalPlayer.statMana += 20;
            }
        }
    }
}
