using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;
using UwUPnP;

namespace UnlimitedMod.system.DiabloItem
{
    public class Tooltips : GlobalItem
    {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            foreach (TooltipLine tooltip in tooltips) {
                if (tooltip.Name == "Damage") {
                    tooltip.Text += $" [{item.GetGlobalItem<DiabloItem>().oldDamage}]-[{(int)(item.GetGlobalItem<DiabloItem>().oldDamage * (item.GetGlobalItem<RollSystem>().upperBound / 100))}]";
                }
            }

            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "lifesteal", $"+{item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage}% Life Stolen Per Hit") { OverrideColor = Color.Red });
            }
            if (item.GetGlobalItem<DiabloItem>().EnhancedDamage > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "enhdmg", $"Lower bound: {item.GetGlobalItem<RollSystem>().lowerBound}, Upper bound: {item.GetGlobalItem<RollSystem>().upperBound}, Actual Roll: {item.GetGlobalItem<RollSystem>().roll}"));
            }
            if (item.GetGlobalItem<DiabloItem>().ColdTag)
            {
                tooltips.Add(new TooltipLine(Mod, "coldtag", "[Cold]") { OverrideColor = Color.Cyan });
            }
            if (item.GetGlobalItem<DiabloItem>().FireTag)
            {
                tooltips.Add(new TooltipLine(Mod, "firetag", "[Fire]") { OverrideColor = Color.OrangeRed });
            }
            if (item.GetGlobalItem<DiabloItem>().PoisonTag)
            {
                tooltips.Add(new TooltipLine(Mod, "poisontag", "[Poison]") { OverrideColor = Color.LimeGreen });
            }
            if (item.GetGlobalItem<DiabloItem>().LightningTag)
            {
                tooltips.Add(new TooltipLine(Mod, "lightningtag", "[Lightning]") { OverrideColor = Color.LightYellow });
            }
        }
    }
}