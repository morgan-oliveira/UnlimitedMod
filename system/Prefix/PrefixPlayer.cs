using System;
using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod {
    public class PrefixPlayer : ModPlayer {
        public override void SetStaticDefaults()
        {

        }
        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (item.Name.Contains("Mana-loved")) {
                item.mana -= (int)Math.Round(item.mana * 0.2f);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HeldItem.Name.Contains("Ninjutsu")) {
                Player.moveSpeed += 0.2f;
            }
            if (Player.HeldItem.Name.Contains("Adventurous")) {
                Level.xpMult = 1.3f;
            }
        }
    }
}