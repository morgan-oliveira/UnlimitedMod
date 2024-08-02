using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
                item.GetGlobalItem<RollSystem>().upperBound = value * 1.3f;
                return item.GetGlobalItem<RollSystem>().upperBound;
            }
            else return -1;
        }  
        // Similarly, this method generates the lowest possible value for the specified value.
        public static float GenerateLowerRangeValue(Item item, float value) {
            if (value > 0) {
                item.GetGlobalItem<RollSystem>().lowerBound = value * 0.6f;
                return item.GetGlobalItem<RollSystem>().lowerBound;
            } 
            else return 1;
        } 
        /* This method generates the actual roll for the specified value, using the values from GenerateLowerRangeValue
        and GenerateUpperRangeValue as bounds for the NextFloat() method. 
        We need to return the value to be able to use it in different methods, such as EnhancedDamageRoll(), SocketRoll(),
        CalculateRES(), etc.    
        We also need to display it in the tooltips.
        */
        public static float GenerateRoll(Item item, float value) {
            item.GetGlobalItem<RollSystem>().roll = Main.rand.NextFloat(GenerateLowerRangeValue(item, value), GenerateUpperRangeValue(item, value)+1);
            return item.GetGlobalItem<RollSystem>().roll;
        }
        /*Generates a random value based on NextFloat. Use it to check whether certain probabilities based on percentages
        were actually met. It is used to generate elemental tags, and to decide whether the item is [Cold], [Poison], etc
        Usage is RollSystem.GeneratePublicRoll("percentage of event happening").
        For instance, to check whether a 40% chance event happened, you may call: 
        [RollSystem.GeneratePublicRoll(0.4f)].
        Returns -1 if the event didn't happen, and 1 if the event happened.
        */
        public static float GeneratePublicRoll(float value) {
            float generator = Main.rand.NextFloat();
            if (generator < value) {
                return 1;
            }
            else return -1;
        }
        public static float GeneratePrefix(Item item, float value) {
            return -1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag["lowerBound"] = lowerBound;
            tag["upperBound"] = upperBound;
            tag["roll"] = roll;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            lowerBound = tag.GetFloat("lowerBound");
            upperBound = tag.GetFloat("upperBound");
            roll = tag.GetFloat("roll");
        }
    }
}