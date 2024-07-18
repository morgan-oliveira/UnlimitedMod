using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system.Prefix
{
    public class PrefixGenerator : GlobalItem
    {
        // Inicializando objeto do tipo PrefixController, que gerencia os dados desserializados do arquivo JSON.
        public PrefixesList PrefixesData { get; private set; } 
        // Inicializando dicionário de ações por atributo, usando como chave os atributos, e como valor o delegate Action<>.
        private Dictionary<string, Action<Item>> attributeActions;
        public List<PrefixController> prefixes;
        public override bool InstancePerEntity => true;


        public override void Load()
        {
            // Carrega o arquivo JSON no caminho especificado
            // O método Combine já coloca as barrinhas '/' do diretório, basta preencher com o nome das pastas
            string jsonFilePath = Path.Combine(ModLoader.ModPath, "Prefixes.JSON");
            if (File.Exists(jsonFilePath))
            {
                // Desserializando o JSON
                string jsonContent = File.ReadAllText(jsonFilePath);
                var rootJson = JsonConvert.DeserializeObject<Dictionary<string, List<PrefixController>>>(jsonContent);
                prefixes = rootJson["prefixes"];
            }
            // Inicializando dicionário associando atributos (atr) com o delegate Action, que recebe como parâmetro
            // quaisquer métodos que usem um parâmetro da classe Item
            // Cada par (atributo, metodo) representa o funcionamento de um prefixo do arquivo JSON
            attributeActions = new Dictionary<string, Action<Item>> {
                { "mana", ApplyMana },
                { "melee", ApplyMelee }
            };  
        }
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (prefixes != null && prefixes.Count > 0)
            {
                var randomm = Main.rand.Next(1, 5);
                if (randomm < 3) {
                    
                }
            }
            ApplyPrefixes(item);
        }

        // TODO: Finalizar funcionamento do método que associa um prefixo a um item
        public void ApplyPrefixes(Item item)
        {
            if (PrefixesData != null && PrefixesData.Prefixes.Count > 0)
            {
                foreach (PrefixController prefix in prefixes)
                {
                    if (attributeActions.ContainsKey(prefix.Atr))
                    {
                        // Chama o método associado ao atributo
                        attributeActions[prefix.Atr](item);
                    }
                }
                item.SetNameOverride($"{prefixes[3]} {item.Name}");
            }
        }
        // Método para aplicar bônus de prefixo para itens melee
        private void ApplyMelee(Item item)
        {
            if (attributeActions.ContainsKey("Berserk"))
            {
                item.GetGlobalItem<PrefixController>().PrefixName = prefixes.GetEnumerator().Current.PrefixName;
                if (item.DamageType == DamageClass.Melee)
                {
                    item.damage = (int)(item.damage * 0.3f);
                    item.useTurn = false;
                    item.useAnimation -= 10;
                    item.useTime -= 10;
                }
            }
        }
        // Método para aplicar bônus de prefixo para itens magic
        private void ApplyMana(Item item)
        {
            if (attributeActions.ContainsKey("Arcane"))
            {
                item.mana += Main.rand.Next(1, 21);
            }
            if (attributeActions.ContainsKey("Hex"))
            {
                item.mana += Main.rand.Next(20, 51);
            }
        }
    }
}
