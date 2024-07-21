using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system.Prefix
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
            ApplyPrefixes(item);
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            Main.NewText($"{item.damage}");
            ApplyPrefixes(item);
        }
        // TODO: Finalizar funcionamento do método que associa um prefixo a um item
        public void ApplyPrefixes(Item item)
        {
            attributeActions = new Dictionary<string, Action<Item>> {
                { "mana", ApplyMana },
                { "melee", ApplyMelee },
                { "magic", ApplyMagic },
                { "knockback", ApplyKnockback },
                { "FCR", ApplyFCR },
                { "atkspd", ApplyATKSPD },
                { "manaCost", ApplyManaCost },
                { "movespd", ApplyMoveSPD }
            };
            
            if (prefixes != null && prefixes.Count > 0)
            {
                Randomizer = Main.rand.Next(0, 17);
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

        private void ApplyMoveSPD(Item item)
        {
            Main.NewText("Tais voando dog");
        }

        private void ApplyManaCost(Item item)
        {
            if (item.mana > 0) {
                item.mana -= (int)(item.mana * 0.5f);
            }
        }

        private void ApplyATKSPD(Item item)
        {
            if (item.DamageType == DamageClass.Melee) {
                item.useTime -= 7;
                item.useAnimation -= 7;
            }
        }

        private void ApplyFCR(Item item)
        {
             if (item.DamageType == DamageClass.Magic) {
                item.useTime -= 5;
                item.useAnimation -= 5;
             }
        }

        private void ApplyKnockback(Item item)
        {
            item.knockBack += 3.5f;
        }

        private void ApplyMagic(Item item)
        {
            if (item.DamageType == DamageClass.Magic) {
                item.damage += (int)(item.damage * 0.25f);
            }
        }

        private void ApplyMelee(Item item)
        {
            if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed) {
                item.damage += (int)(item.damage * 0.25f);
            }
        }

        private void ApplyMana(Item item)
        {
            if (item.mana > 0) {
                Main.LocalPlayer.statMana += 20;
            }
        }
    }
}
