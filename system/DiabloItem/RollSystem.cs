using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system.DiabloItem {
    public class RollSystem : GlobalItem {
        public float upperBound;
        public float lowerBound;
        public float roll;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item entity)
        {
            entity.GetGlobalItem<RollSystem>().upperBound = upperBound;
            entity.GetGlobalItem<RollSystem>().lowerBound = lowerBound;
            entity.GetGlobalItem<RollSystem>().roll = roll;
        }
        public static float GenerateUpperRangeValue(Item item, float value) {
            if (value > 0) {
                item.GetGlobalItem<RollSystem>().upperBound = value * 2;
                return item.GetGlobalItem<RollSystem>().upperBound;
            }
            else return -1;
        }  
        public static float GenerateLowerRangeValue(Item item, float value) {
            if (value > 0) {
                item.GetGlobalItem<RollSystem>().lowerBound = value / 2;
                return item.GetGlobalItem<RollSystem>().lowerBound;
            } 
            else return 1;
        } 
        public static float GenerateRoll(Item item, float value) {
            item.GetGlobalItem<RollSystem>().roll = Main.rand.NextFloat(GenerateLowerRangeValue(item, value), GenerateUpperRangeValue(item, value)+1);
            return item.GetGlobalItem<RollSystem>().roll;
        }
        public void EnhancedDamage(float enhancedDamage, Item item) {
            item.GetGlobalItem<DiabloItem>().EnhancedDamage = enhancedDamage;
            item.damage = (int)(item.damage * (GenerateRoll(item, enhancedDamage) / 100)); 
        }
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            EnhancedDamage(item.GetGlobalItem<DiabloItem>().EnhancedDamage, item);
        }
    }
}