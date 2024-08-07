using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace UnlimitedMod.system
{

    public class Level : ModPlayer
    {
        public static int level = 1;
        public static int xp = 0;
        public static float xpMult = 1;
        public static int xpToLevelUp;
        public static int points;

        public static void SetLevel(int playerLevel)
        {
            level = playerLevel;
        }
        public static void SetXPGain(int XpGain)
        {
            xp += (int)Math.Round(XpGain * xpMult);
        }
        public static int GetLevel()
        {
            return level;
        }
        public static int GetXP()
        {
            return xp;
        }
        public static void LevelUp()
        {
            level += 1;
            points += 2; // To be used in the PlayerAttributes UI
        }
        public override void PreUpdate()
        {
            xpToLevelUp = (int)Math.Pow(level, 3) - (int)Math.Pow(level, 2);
            // If the total XP gained is bigger than the set amount required to level up, then the player levels up
            if (xp >= xpToLevelUp)
            {
                LevelUp();
                xp = 0;
            }
        }

    }
}

