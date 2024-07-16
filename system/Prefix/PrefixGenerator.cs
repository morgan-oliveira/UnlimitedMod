using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system.Prefix
{
    public class PrefixGenerator : Mod
    {
        // Inicializando objeto do tipo PrefixController, que gerencia os dados desserializados do arquivo JSON.
        public PrefixController PrefixesData { get; private set; } 
        // Inicializando dicionário de ações por atributo, usando como chave os atributos, e como valor o delegate Action<>.
        private Dictionary<string, Action<Item>> attributeActions;


        public override void Load()
        {
            // Carrega o arquivo JSON no caminho especificado
            // O método Combine já coloca as barrinhas '/' do diretório, basta preencher com o nome das pastas
            string jsonFilePath = Path.Combine(ModLoader.ModPath, "system", "Prefix", "Prefixes.JSON");
            if (File.Exists(jsonFilePath))
            {
                // Desserializando o JSON
                string jsonContent = File.ReadAllText(jsonFilePath);
                PrefixesData = JsonConvert.DeserializeObject<PrefixController>(jsonContent);
            }
            // Inicializando dicionário associando atributos (atr) com o delegate Action, que recebe como parâmetro
            // quaisquer métodos que usem um parâmetro da classe Item
            // Cada par (atributo, metodo) representa o funcionamento de um prefixo do arquivo JSON
            attributeActions = new Dictionary<string, Action<Item>> {
                { "mana", ApplyMana },
                { "melee", ApplyMelee }
            };
        }

        // TODO: Finalizar funcionamento do método que associa um prefixo a um item
        public void ApplyPrefixes(Item item, string prefixId, Dictionary<string, PrefixController> prefixesData)
        {
            if (prefixesData != null && prefixesData.ContainsKey(prefixId))
            {
                var prefix = prefixesData[prefixId];
                foreach (var attribute in prefix.Attributes)
                {
                    if (attributeActions.ContainsKey(attribute))
                    {
                        // Chama o método associado ao atributo
                        attributeActions[attribute](item);
                    }
                }
                item.SetNameOverride($"{prefix.Name} {item.Name}");
            }
        }
        // Método para aplicar bônus de prefixo para itens melee
        private void ApplyMelee(Item item)
        {
            if (attributeActions.ContainsKey("Berserk"))
            {
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
