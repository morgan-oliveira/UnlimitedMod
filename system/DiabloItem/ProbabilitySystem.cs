using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace UnlimitedMod.system.DiabloItem {
    public class ProbabilitySystem : ModSystem {
        private static float sum;
        public static float[] ProbabilitySpace = { 0.1f, 0.2f, 0.3f, 0.09f, 0.12f };
        public static float[] DozenProbabilitySpace = { 0.02f, 0.04f, 0.06f, 0.08f, 0.1f, 0.12f, 0.14f, 0.16f, 0.18f, 0.1f };
        public static float[] RareItemPool = {0.000001f, 0.000003f, 0.000006f, 0.00001f, 0.000015f, 
            0.000021f, 0.000028f, 0.000036f, 0.000045f, 0.999835f};
        float chance;
        public static int CalculateBaseProbabilities() {
            float rand = Main.rand.NextFloat();
            sum = 0;

            for (int i = 0; i < ProbabilitySpace.Length; i++) {
                sum += ProbabilitySpace[i];
                if (rand < sum) {
                    return i;
                }
            }
            return ProbabilitySpace.Length - 1;
           // Main.NewText($"Is this function being fucking called? {sum}");
        }

    }
}