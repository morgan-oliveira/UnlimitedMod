using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

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
        public int oldDamage {get; set;}
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
            entity.GetGlobalItem<DiabloItem>().oldDamage = oldDamage;
        }
        public override bool InstancePerEntity => true;
        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            // Removing the old vanilla prefix system
            if (pre == -1) {
                return false;
            } return false;
        }
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
                CrushingBlow(CrushingBlowChance, hit, target);
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
                ChanceToCastSkill(ProjID, ChanceToCast, item);
            }
            // If the item has a certain tag, it will apply specific effects on hit
            if (item.GetGlobalItem<DiabloItem>().ColdTag) {
                // Applies Frostburn
                target.AddBuff(BuffID.Frostburn, 180); 
                // Final damage output is reduced based on resistance
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().ColdResistance); 
            }
            if (item.GetGlobalItem<DiabloItem>().PoisonTag) {
                target.AddBuff(BuffID.Poisoned, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().PoisonResistance); 
            }
            if (item.GetGlobalItem<DiabloItem>().FireTag) {
                target.AddBuff(BuffID.OnFire, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().FireResistance); 
            }
            if (item.GetGlobalItem<DiabloItem>().LightningTag) {
                target.AddBuff(BuffID.Electrified, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().LightningResistance); 
            }

        }

        private void ChanceToCastSkill(int projectile, float chance, Item item) {
            
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            item.GetGlobalItem<DiabloItem>().oldDamage = item.OriginalDamage;
            // Calculate Enhanced Damage
            //EnhancedDamageRoll(item.GetGlobalItem<DiabloItem>().EnhancedDamage, item);
            // Calculate Enhanced Defense
            //EnhancedDefenseRoll(item.GetGlobalItem<DiabloItem>().EnhancedDefense, item);
            // Handles Elemental Tags
            GenerateElementalTag(item);
            
        }

        private void CalculatePoisonRES(float poisonResistance)
        {
            //throw new NotImplementedException();
        }

        private void CalculateLightningRES(float lightningResistance)
        {
            //throw new NotImplementedException();
        }

        private void CalculateFireRES(float fireResistance)
        {
            //throw new NotImplementedException();
        }

        private void CalculateColdRES(float coldResistance)
        {
            //throw new NotImplementedException();
        }

        private void DeadlyStrike(float deadlyStrikeChance)
        {
            throw new NotImplementedException();
        }

        private void OpenWounds(float openWoundsChance)
        {
            throw new NotImplementedException();
        }

        private void CrushingBlow(float crushingBlowChance, NPC.HitInfo hit, NPC target)
        {
            //throw new NotImplementedException();

            hit.Damage += (int)(target.lifeMax * 0.25f);
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
            // Math.Ceiling actually rounds float to the highest value, but we need an int casting because it returns a double
            // We can round to the lowest value using Math.Floor, the same rules apply
            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0) {
                player.statLife += (int)Math.Ceiling(item.damage * (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage / 100));
            }
        }
        public void EnhancedDamageRoll(float enhancedDamage, Item item) {
            item.GetGlobalItem<DiabloItem>().EnhancedDamage = enhancedDamage;
            item.GetGlobalItem<DiabloItem>().oldDamage = item.damage;
            //item.damage = (int)(item.damage * (RollSystem.GenerateRoll(item, enhancedDamage) / 100)); 
        }
        public void EnhancedDefenseRoll(float enhancedDefense, Item item) {
            item.GetGlobalItem<DiabloItem>().EnhancedDefense = enhancedDefense;
            if (item.defense > 0) {
                item.defense = (int)(item.defense * (RollSystem.GenerateRoll(item, enhancedDefense) / 100));
            }
        }
        #endregion

        #region ElementalTags
        public void GenerateElementalTag(Item item) {
            if (RollSystem.GeneratePublicRoll(0.3f) == 1) {
                int selector = Main.rand.Next(0,4);
                switch (selector) {
                    case 0: item.GetGlobalItem<DiabloItem>().ColdTag = true;
                    break;
                    case 1: item.GetGlobalItem<DiabloItem>().FireTag = true;
                    break;
                    case 2: item.GetGlobalItem<DiabloItem>().LightningTag = true;
                    break;
                    case 3: item.GetGlobalItem<DiabloItem>().PoisonTag = true;
                    break;
                }
            }
        }
        #endregion
    }
}
 