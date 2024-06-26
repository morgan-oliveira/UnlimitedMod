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
        // Generates the highest possible value for the specified property. 
        // Used to calculate the highest bound for the specified attribute's value
        public static float GenerateUpperRangeValue(Item item, float value) {
            if (value > 0) {
                item.GetGlobalItem<RollSystem>().upperBound = value * 2;
                return item.GetGlobalItem<RollSystem>().upperBound;
            }
            else return -1;
        }  
        // Similarly, this method generates the lowest possible value for the specified value.
        public static float GenerateLowerRangeValue(Item item, float value) {
            if (value > 0) {
                item.GetGlobalItem<RollSystem>().lowerBound = value / 2;
                return item.GetGlobalItem<RollSystem>().lowerBound;
            } 
            else return 1;
        } 
        /* This method generates the actual roll for the specified value, using the values from GenerateLowerRangeValue
        and GenerateUpperRangeValue as bounds for the NextFloat() method. 
        We need to return the value to be able to use it in different methods, such as EnhancedDamageRoll(), SocketRoll(),
        CalculateRES(), etc.    
        */
        public static float GenerateRoll(Item item, float value) {
            item.GetGlobalItem<RollSystem>().roll = Main.rand.NextFloat(GenerateLowerRangeValue(item, value), GenerateUpperRangeValue(item, value)+1);
            return item.GetGlobalItem<RollSystem>().roll;
        }
    }
}