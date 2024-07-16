using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Newtonsoft.Json;

namespace UnlimitedMod.system.DiabloItem
{
    // Classe responsável por gerenciar arquivo JSON de prefixos
    public class PrefixController
    {
        // Busca a propriedade chamada "name" no arquivo JSON que chamar esta classe
        [JsonProperty("name")]
        public string Name { get; set; }    
        // Faz de forma similar, buscando a propriedade "atr"
        [JsonProperty("atr")]
        public string Atr {get; set;}

        // Retorna uma lista de propriedades do tipo "atr", aceitando inclusive atrs múltiplos (separados por vírgula)
        public List<string> Attributes {
            get {
                return new List<string>(Atr.Split(','));
            }
        }
    }
}