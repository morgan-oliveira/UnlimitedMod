using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system.DiabloItem {
    public class Tooltips : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0) {
                tooltips.Add(new TooltipLine(Mod, "lifesteal", $"+{item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage}% Life Stolen Per Hit") {OverrideColor = Color.Red} );
            }
        }
    }
}