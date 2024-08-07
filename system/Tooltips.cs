using System;
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

            foreach (TooltipLine tooltip in tooltips)
            {
                if (tooltip.Name == "Damage")
                {
                    tooltip.Text += $" [{item.GetGlobalItem<RollSystem>().rolledDamage}] [{item.GetGlobalItem<RollSystem>().damageRange.Min}]-[{item.GetGlobalItem<RollSystem>().damageRange.Max}]";
                    if (item.GetGlobalItem<DiabloItem>().PoisonTag) {

                        TooltipLine poisontooltip = new TooltipLine(Mod, "poisondmg", $"{item.GetGlobalItem<DiabloItem>().PoisonDamage} poison damage") { OverrideColor = Color.LimeGreen };
                        tooltips.Insert(2, poisontooltip);
                        break;
                    }
                    if (item.GetGlobalItem<DiabloItem>().FireTag) {

                        TooltipLine firetooltip = new TooltipLine(Mod, "firedmg", $"{item.GetGlobalItem<DiabloItem>().FireDamage} fire damage") { OverrideColor = Color.OrangeRed };
                        tooltips.Insert(2, firetooltip);
                        break;
                    }
                    if (item.GetGlobalItem<DiabloItem>().ColdTag) {

                        TooltipLine coldtooltip = new TooltipLine(Mod, "colddmg", $"{item.GetGlobalItem<DiabloItem>().ColdDamage} cold damage") { OverrideColor = Color.Cyan };
                        tooltips.Insert(2, coldtooltip);
                        break;
                    }
                    if (item.GetGlobalItem<DiabloItem>().LightningTag) {

                        TooltipLine lightningtooltip = new TooltipLine(Mod, "lightningdmg", $"{item.GetGlobalItem<DiabloItem>().LightningDamage} lightning damage") { OverrideColor = Color.LightYellow };
                        tooltips.Insert(2, lightningtooltip);
                        break;
                    }

                }
            }

            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "lifesteal", $"+{item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage}% Life Stolen Per Hit") { OverrideColor = Color.Red });
            }
            if (item.GetGlobalItem<DiabloItem>().EnhancedDamage > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "enhdmg", $"+{Math.Round(item.GetGlobalItem<RollSystem>().rolledEnhancedDamage, 0)}% Enhanced Damage [{item.GetGlobalItem<RollSystem>().enhdmgRange.Min}]-[{item.GetGlobalItem<RollSystem>().enhdmgRange.Max}]"){ OverrideColor = Color.Blue });
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
            if (item.GetGlobalItem<PrefixController>().Atr == "poisondmg") {

            }
        }
    }
}