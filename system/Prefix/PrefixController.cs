using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Newtonsoft.Json;
using System;

namespace UnlimitedMod.system.DiabloItem
{
    // Classe responsável por gerenciar arquivo JSON de prefixos
    public class PrefixController : GlobalItem
    {
        
        // Busca a propriedade chamada "name" no arquivo JSON que chamar esta classe
        [JsonProperty("name")]
        public string PrefixName { get; set; }    
        // Faz de forma similar, buscando a propriedade "atr"
        [JsonProperty("atr")]
        public string Atr {get; set;}
        [JsonProperty("id")]
        public int Id {get; set;}

        // Retorna uma lista de propriedades do tipo "atr", aceitando inclusive atrs múltiplos (separados por vírgula)
        public List<string> Attributes {
            get {
                return new List<string>(Atr.Split(','));
            }
        }
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item entity)
        {
            entity.GetGlobalItem<PrefixController>().PrefixName = PrefixName;
            entity.GetGlobalItem<PrefixController>().Atr = Atr;
            entity.GetGlobalItem<PrefixController>().Id = Id;
        }
    }
}