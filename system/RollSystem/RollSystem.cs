using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

public class RollSystem : GlobalItem
{
    public bool InventoryUpdated = false;
    public int rolledDamage;
    public float rolledEnhancedDamage;
    public float rolledEnhancedDefense;
    public float rolledFasterCastRate;
    public float rolledFasterWalkRun;
    public int rolledSockets;
    public int rolledMF;
    public float rolledColdResistance;
    public float rolledFireResistance;
    public float rolledPoisonResistance;
    public float rolledLightningResistance;
    public float rolledPhysicalResistance;
    public float rolledOpenWoundsChance;
    public float rolledDeadlyStrikeChance;
    public float rolledCrushingBlowChance;
    public int rolledProjID;
    public float rolledChanceToCast;
    public float rolledLifeStolenPerHitPercentage;
    public float rolledManaStolenPerHitPercentage;
    public int rolledOldDamage;
    public int rolledColdDamage;
    public int rolledFireDamage;
    public int rolledPoisonDamage;
    public int rolledLightningDamage;
    public RollRange damageRange = new RollRange(0, 0);
    public RollRange enhdmgRange = new RollRange(0, 0);
    public RollRange enhdefRange;
    public float lowRange = 0.6f;
    public float highRange = 1.3f;
    public override bool InstancePerEntity => true;
    public override void SetDefaults(Item entity)
    {
        entity.GetGlobalItem<RollSystem>().rolledEnhancedDamage = rolledEnhancedDamage;
        entity.GetGlobalItem<RollSystem>().rolledEnhancedDefense = rolledEnhancedDefense;
        entity.GetGlobalItem<RollSystem>().rolledFasterCastRate = rolledFasterCastRate;
        entity.GetGlobalItem<RollSystem>().rolledFasterWalkRun = rolledFasterWalkRun;
        entity.GetGlobalItem<RollSystem>().rolledSockets = rolledSockets;
        entity.GetGlobalItem<RollSystem>().rolledMF = rolledMF;
        entity.GetGlobalItem<RollSystem>().rolledColdResistance = rolledColdResistance;
        entity.GetGlobalItem<RollSystem>().rolledFireResistance = rolledFireResistance;
        entity.GetGlobalItem<RollSystem>().rolledPoisonResistance = rolledPoisonResistance;
        entity.GetGlobalItem<RollSystem>().rolledLightningResistance = rolledLightningResistance;
        entity.GetGlobalItem<RollSystem>().rolledPhysicalResistance = rolledPhysicalResistance;
        entity.GetGlobalItem<RollSystem>().rolledOpenWoundsChance = rolledOpenWoundsChance;
        entity.GetGlobalItem<RollSystem>().rolledDeadlyStrikeChance = rolledDeadlyStrikeChance;
        entity.GetGlobalItem<RollSystem>().rolledCrushingBlowChance = rolledCrushingBlowChance;
        entity.GetGlobalItem<RollSystem>().rolledProjID = rolledProjID;
        entity.GetGlobalItem<RollSystem>().rolledChanceToCast = rolledChanceToCast;
        entity.GetGlobalItem<RollSystem>().rolledLifeStolenPerHitPercentage = rolledLifeStolenPerHitPercentage;
        entity.GetGlobalItem<RollSystem>().rolledManaStolenPerHitPercentage = rolledManaStolenPerHitPercentage;
        entity.GetGlobalItem<RollSystem>().rolledOldDamage = rolledOldDamage;
        entity.GetGlobalItem<RollSystem>().rolledColdDamage = rolledColdDamage;
        entity.GetGlobalItem<RollSystem>().rolledFireDamage = rolledFireDamage;
        entity.GetGlobalItem<RollSystem>().rolledPoisonDamage = rolledPoisonDamage;
        entity.GetGlobalItem<RollSystem>().rolledLightningDamage = rolledLightningDamage;
    }
    public int RollValue(RollRange range)
    {
        return Main.rand.Next(range.Min, range.Max + 1);
    }
    public RollRange CalculateRollRange(int originalValue, float lowerBound, float upperBound)
    {
        int min = (int)Math.Round(originalValue * lowerBound);
        int max = (int)Math.Round(originalValue * upperBound);
        return new RollRange(min, max);
    }
    public void DamageRoll(Item item)
    {
        if (item.damage > 0)
        {
            damageRange = CalculateRollRange(item.OriginalDamage, lowRange, highRange);
            item.GetGlobalItem<RollSystem>().rolledDamage = RollValue(damageRange);
            item.damage = item.GetGlobalItem<RollSystem>().rolledDamage;
        }
    }
    public void EnhancedDamageRoll(Item item) {
        if (item.GetGlobalItem<PrefixController>().Atr != "" && item.GetGlobalItem<PrefixController>().Atr.Contains("enhdmg") ) {
            item.GetGlobalItem<DiabloItem>().EnhancedDamage = Main.rand.NextFloat() * 100;
            enhdmgRange = CalculateRollRange((int)item.GetGlobalItem<DiabloItem>().EnhancedDamage, lowRange, highRange);
            item.GetGlobalItem<RollSystem>().rolledEnhancedDamage = RollValue(enhdmgRange);
        }
    }
    public void EnhancedDefenseRoll(Item item) {

    }
    public override void OnCreated(Item item, ItemCreationContext context)
    {
        DamageRoll(item);
        EnhancedDamageRoll(item);
        //Main.NewText($"{RollValue(item)}");
        Main.NewText($"{item.GetGlobalItem<RollSystem>().rolledDamage}");

    }
    public override void UpdateInventory(Item item, Player player)
    {
        if (!InventoryUpdated)
        {
            DamageRoll(item);
            EnhancedDamageRoll(item);
            if (rolledDamage > 0)
            {
                InventoryUpdated = true;
            }
        }
    }
    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        damage += item.GetGlobalItem<RollSystem>().rolledEnhancedDamage / 100;
    }
}