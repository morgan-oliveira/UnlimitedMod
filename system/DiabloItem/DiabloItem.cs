using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace UnlimitedMod.system.DiabloItem {
    public class DiabloItem : GlobalItem {
        #region SetAttributes
        public int Dmg {get; set;}
        public float EnhancedDamage {get; set;}
        public float EnhancedDefense {get; set;}
        public float FasterCastRate {get; set;}
        public float FasterRunWalk {get; set;}
        public int Sockets {get; set;}
        public MagicFind MF {get; set;}
        public bool ColdTag {get; set;}
        public bool FireTag {get; set;}
        public bool PoisonTag {get; set;}
        public bool LightningTag {get; set;}
        public float ColdResistance {get; set;}
        public float FireResistance {get; set;}
        public float PoisonResistance {get; set;}
        public float LightningResistance {get; set;}
        public float PhysicalResistance {get; set;}
        public bool HasOpenWounds {get; set;}
        public bool HasDeadlyStrike {get; set;}
        public bool HasCrushingBlow {get; set;}
        public float OpenWoundsChance {get; set;}
        public float DeadlyStrikeChance {get; set;}
        public float CrushingBlowChance {get; set;}
        public bool HasChanceToCast {get; set;}
        public int ProjID {get; set;}
        public float ChanceToCast {get; set;}
        public bool CannotBeFrozen {get; set;}
        public float LifeStolenPerHitPercentage {get; set;}
        public float ManaStolenPerHitPercentage {get; set;}
        #endregion

        public override void SetDefaults(Item entity) {
            entity.GetGlobalItem<DiabloItem>().Dmg = entity.damage;
            entity.GetGlobalItem<DiabloItem>().EnhancedDamage = EnhancedDamage;
            entity.GetGlobalItem<DiabloItem>().EnhancedDefense = EnhancedDefense;
            entity.GetGlobalItem<DiabloItem>().FasterCastRate = FasterCastRate;
            entity.GetGlobalItem<DiabloItem>().FasterRunWalk = FasterRunWalk;
            entity.GetGlobalItem<DiabloItem>().Sockets = Sockets;
            entity.GetGlobalItem<DiabloItem>().MF = MF;
            entity.GetGlobalItem<DiabloItem>().ColdTag = ColdTag;
            entity.GetGlobalItem<DiabloItem>().FireTag = FireTag;
            entity.GetGlobalItem<DiabloItem>().PoisonTag = PoisonTag;
            entity.GetGlobalItem<DiabloItem>().LightningTag = LightningTag;
            entity.GetGlobalItem<DiabloItem>().ColdResistance = ColdResistance;
            entity.GetGlobalItem<DiabloItem>().FireResistance = FireResistance;
            entity.GetGlobalItem<DiabloItem>().PoisonResistance = PoisonResistance;
            entity.GetGlobalItem<DiabloItem>().LightningResistance = LightningResistance;
            entity.GetGlobalItem<DiabloItem>().PhysicalResistance = PhysicalResistance;
            entity.GetGlobalItem<DiabloItem>().HasOpenWounds = HasOpenWounds;
            entity.GetGlobalItem<DiabloItem>().HasCrushingBlow = HasCrushingBlow;
            entity.GetGlobalItem<DiabloItem>().HasDeadlyStrike = HasDeadlyStrike;
            entity.GetGlobalItem<DiabloItem>().OpenWoundsChance = OpenWoundsChance;
            entity.GetGlobalItem<DiabloItem>().CrushingBlowChance = CrushingBlowChance;
            entity.GetGlobalItem<DiabloItem>().DeadlyStrikeChance = DeadlyStrikeChance;
            entity.GetGlobalItem<DiabloItem>().ProjID = ProjID;
            entity.GetGlobalItem<DiabloItem>().ChanceToCast = ChanceToCast;
            entity.GetGlobalItem<DiabloItem>().CannotBeFrozen = CannotBeFrozen;
            entity.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage = LifeStolenPerHitPercentage;
            entity.GetGlobalItem<DiabloItem>().ManaStolenPerHitPercentage = ManaStolenPerHitPercentage;
        }
        public override bool InstancePerEntity => true;

        #region SpecialAttributes
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Life stolen per hit
            LifeStolenPerHit(player, item);

            // Mana stolen per hit
            ManaStolenPerHit(player, item);

            // Calculate Resistances
            CalculateColdRES(ColdResistance);
            CalculateFireRES(FireResistance);
            CalculateLightningRES(LightningResistance);
            CalculatePoisonRES(PoisonResistance);

            // Crushing Blow
            if (HasCrushingBlow) {
                CrushingBlow(CrushingBlowChance);
            }

            // Open Wounds
            if (HasOpenWounds) {
                OpenWounds(OpenWoundsChance);
            }

            // Deadly Strike
            if (HasDeadlyStrike) {
                DeadlyStrike(DeadlyStrikeChance);
            }

            // Chance to cast Waterbolt on hit
            if (HasChanceToCast) {
                ChanceToCastSkill(ProjID, ChanceToCast);
            }

        }

        private void ChanceToCastSkill(int projectile, float chance)
        {

        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            // THIS IS WHERE WE CREATE AN INSTANCE OF ProbabilitySystem CLASS
            // Calculate Enhanced Damage
            
            // Calculate Enhanced Defense

            // Set Boolean variables to True, depending on their roll to Casting
        }

        private void CalculatePoisonRES(float poisonResistance)
        {
            throw new NotImplementedException();
        }

        private void CalculateLightningRES(float lightningResistance)
        {
            throw new NotImplementedException();
        }

        private void CalculateFireRES(float fireResistance)
        {
            throw new NotImplementedException();
        }

        private void CalculateColdRES(float coldResistance)
        {
            throw new NotImplementedException();
        }

        private void DeadlyStrike(float deadlyStrikeChance)
        {
            throw new NotImplementedException();
        }

        private void OpenWounds(float openWoundsChance)
        {
            throw new NotImplementedException();
        }

        private void CrushingBlow(float crushingBlowChance)
        {
            throw new NotImplementedException();
        }

        private void ManaStolenPerHit(Player player, Item item)
        {
            //throw new NotImplementedException();
            // Implementing similarly to LifeStolenPerHit
            if (item.GetGlobalItem<DiabloItem>().ManaStolenPerHitPercentage > 0) {
                player.statMana += (int)Math.Ceiling(item.damage * (item.GetGlobalItem<DiabloItem>().ManaStolenPerHitPercentage / 100));
            }
        }

        private void LifeStolenPerHit(Player player, Item item)
        {
            // LifeSteal is a float value, so we set its % relative to the item's damage
            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0) {
                player.statLife += (int)Math.Ceiling(item.damage * (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage / 100));
            }
        }
        #endregion

    }
}