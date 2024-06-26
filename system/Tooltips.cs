using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;
using UwUPnP;

namespace UnlimitedMod.system.DiabloItem {
    public class Tooltips : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0) {
                tooltips.Add(new TooltipLine(Mod, "lifesteal", $"+{item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage}% Life Stolen Per Hit") {OverrideColor = Color.Red} );
            }
            if (item.GetGlobalItem<DiabloItem>().EnhancedDamage > 0) {
                tooltips.Add(new TooltipLine(Mod, "enhdmg", $"Lower bound: {item.GetGlobalItem<RollSystem>().lowerBound}, Upper bound: {item.GetGlobalItem<RollSystem>().upperBound}, Actual Roll: {item.GetGlobalItem<RollSystem>().roll}"));
            }
        }
    }
}