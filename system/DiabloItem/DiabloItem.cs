using System;
using System.IO;
using System.Threading;
using System.Timers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace UnlimitedMod.system.DiabloItem
{
    public class DiabloItem : GlobalItem
    {
        #region SetAttributes
        public int Dmg { get; set; }
        public float EnhancedDamage { get; set; }
        public float EnhancedDefense { get; set; }
        public float FasterCastRate { get; set; }
        public float FasterRunWalk { get; set; }
        public int Sockets { get; set; }
        public int MF { get; set; }
        public bool ColdTag { get; set; }
        public bool FireTag { get; set; }
        public bool PoisonTag { get; set; }
        public bool LightningTag { get; set; }
        public float ColdResistance { get; set; }
        public float FireResistance { get; set; }
        public float PoisonResistance { get; set; }
        public float LightningResistance { get; set; }
        public float PhysicalResistance { get; set; }
        public bool HasOpenWounds { get; set; }
        public bool HasDeadlyStrike { get; set; }
        public bool HasCrushingBlow { get; set; }
        public float OpenWoundsChance { get; set; }
        public float DeadlyStrikeChance { get; set; }
        public float CrushingBlowChance { get; set; }
        public bool HasChanceToCast { get; set; }
        public int ProjID { get; set; }
        public float ChanceToCast { get; set; }
        public bool CannotBeFrozen { get; set; }
        public float LifeStolenPerHitPercentage { get; set; }
        public float ManaStolenPerHitPercentage { get; set; }
        public int oldDamage { get; set; }
        public int ColdDamage { get; set; }
        public int FireDamage { get; set; }
        public int PoisonDamage { get; set; }
        public int LightningDamage { get; set; }
        #endregion

        public override void SetDefaults(Item entity)
        {
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
            entity.GetGlobalItem<DiabloItem>().ColdDamage = ColdDamage;
            entity.GetGlobalItem<DiabloItem>().FireDamage = FireDamage;
            entity.GetGlobalItem<DiabloItem>().PoisonDamage = PoisonDamage;
            entity.GetGlobalItem<DiabloItem>().LightningDamage = LightningDamage;
        }
        public override bool InstancePerEntity => true;
        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            // Removing the old vanilla prefix system
            if (pre == -1)
            {
                return false;
            }
            return false;
        }
        #region SpecialAttributes
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            damage.Base += item.GetGlobalItem<RollSystem>().rolledFireDamage + item.GetGlobalItem<RollSystem>().rolledColdDamage + item.GetGlobalItem<RollSystem>().rolledLightningDamage + item.GetGlobalItem<RollSystem>().rolledPoisonDamage;
        }
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
            if (HasCrushingBlow)
            {
                CrushingBlow(CrushingBlowChance, hit, target);
            }

            // Open Wounds
            if (HasOpenWounds)
            {
                OpenWounds(OpenWoundsChance);
            }

            // Deadly Strike
            if (HasDeadlyStrike)
            {
                DeadlyStrike(DeadlyStrikeChance);
            }

            // Chance to cast Waterbolt on hit
            if (HasChanceToCast)
            {
                ChanceToCastSkill(ProjID, ChanceToCast, item);
            }
            // If the item has a certain tag, it will apply specific effects on hit
            if (item.GetGlobalItem<DiabloItem>().ColdTag)
            {
                // Applies Frostburn
                target.AddBuff(BuffID.Frostburn, 180);
                // Final damage output is reduced based on resistance
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().ColdResistance);
            }
            if (item.GetGlobalItem<DiabloItem>().PoisonTag)
            {
                target.AddBuff(BuffID.Poisoned, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().PoisonResistance);
            }
            if (item.GetGlobalItem<DiabloItem>().FireTag)
            {
                target.AddBuff(BuffID.OnFire, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().FireResistance);
            }
            if (item.GetGlobalItem<DiabloItem>().LightningTag)
            {
                target.AddBuff(BuffID.Electrified, 180);
                hit.Damage -= (int)(item.damage * item.GetGlobalItem<DiabloItem>().LightningResistance);
            }

        }

        private void ChanceToCastSkill(int projectile, float chance, Item item)
        {
            if (!HasChanceToCast)
            {

            }
        }
        public override void UseAnimation(Item item, Player player)
        {
            ChanceToCastSkill(ProjID, ChanceToCast, item);
        }
        public override bool? UseItem(Item item, Player player)
        {
            //item.shoot = ProjectileID.None;
            return true;
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            //if (RollSystem.GeneratePublicRoll(item.GetGlobalItem<DiabloItem>().ChanceToCast) == 1)
            {
                item.shoot = item.GetGlobalItem<DiabloItem>().ProjID;
                item.shootSpeed = 10f;
            }
        }

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            item.GetGlobalItem<DiabloItem>().oldDamage = item.OriginalDamage;
            RollInitialDamage(item);
            GenerateElementalTag(item);
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            RollInitialDamage(item);
        }
        public static void RollInitialDamage(Item item)
        {

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

        public static void CrushingBlow(float crushingBlowChance, NPC.HitInfo hit, NPC target)
        {
            hit.Damage += (int)(target.lifeMax * 0.25f);
        }

        public static void ManaStolenPerHit(Player player, Item item)
        {
            if (item.GetGlobalItem<DiabloItem>().ManaStolenPerHitPercentage > 0)
            {
                player.statMana += (int)Math.Ceiling(item.damage * (item.GetGlobalItem<DiabloItem>().ManaStolenPerHitPercentage / 100));
            }
        }

        public static void LifeStolenPerHit(Player player, Item item)
        {
            // LifeSteal is a float value, so we set its % relative to the item's damage
            // Math.Ceiling actually rounds float to the highest value, but we need an int casting because it returns a double
            // We can round to the lowest value using Math.Floor, the same rules apply
            if (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage > 0)
            {
                player.statLife += (int)Math.Ceiling(item.damage * (item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage / 100));
            }
        }
        public void EnhancedDefenseRoll(float enhancedDefense, Item item)
        {
            item.GetGlobalItem<DiabloItem>().EnhancedDefense = enhancedDefense;
            if (item.defense > 0)
            {
                //item.defense = (int)(item.defense * (RollSystem.GenerateRoll(item, enhancedDefense) / 100));
            }
        }
        public static void GeneratePoisonDamage(Item item)
        {

        }

        public static bool ValidateItem(Item item)
        {
            if (item.damage > 0 || item.defense > 0 || item.accessory)
            {
                return true;
            }
            else return false;
        }

        public static void ApplyPoisonTag(Item item)
        {
            item.GetGlobalItem<DiabloItem>().PoisonTag = true;
            item.GetGlobalItem<DiabloItem>().LightningTag = false;
            item.GetGlobalItem<DiabloItem>().FireTag = false;
            item.GetGlobalItem<DiabloItem>().ColdTag = false;
        }

        public static void ApplyCritChance(Item item)
        {
            item.crit += 10;
        }

        public static void ApplyJumpSPD(Item item)
        {
            //throw new NotImplementedException();
            Main.NewText("Tais pulandinho hein dog");
        }

        public static void ApplyEnhancedDamage(Item item)
        {
            //item.GetGlobalItem<DiabloItem>().EnhancedDamage = RollSystem.GenerateRoll(item, item.damage);
        }

        public static void ApplyMoveSPD(Item item)
        {
            Main.NewText("Tais voando dog");
        }

        public static void ApplyManaCost(Item item)
        {
            if (item.mana > 0)
            {
                item.mana -= (int)(item.mana * 0.5f);
            }
        }

        public static void ApplyATKSPD(Item item)
        {
            if (item.DamageType == DamageClass.Melee)
            {
                item.useTime -= 7;
                item.useAnimation -= 7;
            }
        }

        public static void ApplyFCR(Item item)
        {
            if (item.DamageType == DamageClass.Magic)
            {
                item.useTime -= 5;
                item.useAnimation -= 5;
            }
        }

        public static void ApplyKnockback(Item item)
        {
            item.knockBack += 3.5f;
        }

        public static void ApplyMana(Item item)
        {
            if (item.mana > 0)
            {

            }
        }
        #endregion

        #region ElementalTags
        public static void GenerateElementalTag(Item item)
        {
            
        }
        #endregion
        #region TagCompoundSave
        public override void SaveData(Item item, TagCompound tag)
        {
            tag["EnhancedDamage"] = EnhancedDamage;
            tag["EnhancedDefense"] = EnhancedDefense;
            tag["FasterCastRate"] = FasterCastRate;
            tag["FasterRunWalk"] = FasterRunWalk;
            tag["Sockets"] = Sockets;
            tag["MF"] = MF;
            tag["ColdTag"] = ColdTag;
            tag["FireTag"] = FireTag;
            tag["PoisonTag"] = PoisonTag;
            tag["LightningTag"] = LightningTag;
            tag["ColdResistance"] = ColdResistance;
            tag["FireResistance"] = FireResistance;
            tag["PoisonResistance"] = PoisonResistance;
            tag["LightningResistance"] = LightningResistance;
            tag["PhysicalResistance"] = PhysicalResistance;
            tag["HasOpenWounds"] = HasOpenWounds;
            tag["HasDeadlyStrike"] = HasDeadlyStrike;
            tag["HasCrushingBlow"] = HasCrushingBlow;
            tag["OpenWoundsChance"] = OpenWoundsChance;
            tag["DeadlyStrikeChance"] = DeadlyStrikeChance;
            tag["CrushingBlowChance"] = CrushingBlowChance;
            tag["HasChanceToCast"] = HasChanceToCast;
            tag["ProjID"] = ProjID;
            tag["ChanceToCast"] = ChanceToCast;
            tag["CannotBeFrozen"] = CannotBeFrozen;
            tag["LifeStolenPerHitPercentage"] = LifeStolenPerHitPercentage;
            tag["ManaStolenPerHitPercentage"] = ManaStolenPerHitPercentage;
            tag["oldDamage"] = oldDamage;
            tag["ColdDamage"] = ColdDamage;
            tag["FireDamage"] = FireDamage;
            tag["PoisonDamage"] = PoisonDamage;
            tag["LightningDamage"] = LightningDamage;
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            EnhancedDamage = tag.GetFloat("EnhancedDamage");
            EnhancedDefense = tag.GetFloat("EnhancedDefense");
            FasterCastRate = tag.GetFloat("FasterCastRate");
            FasterRunWalk = tag.GetFloat("FasterRunWalk");
            Sockets = tag.GetInt("Sockets");
            MF = tag.GetInt("MF");
            ColdTag = tag.GetBool("ColdTag");
            FireTag = tag.GetBool("FireTag");
            PoisonTag = tag.GetBool("PoisonTag");
            LightningTag = tag.GetBool("LightningTag");
            ColdResistance = tag.GetFloat("ColdResistance");
            FireResistance = tag.GetFloat("FireResistance");
            PoisonResistance = tag.GetFloat("PoisonResistance");
            LightningResistance = tag.GetFloat("LightningResistance");
            PhysicalResistance = tag.GetFloat("PhysicalResistance");
            HasOpenWounds = tag.GetBool("HasOpenWounds");
            HasDeadlyStrike = tag.GetBool("HasDeadlyStrike");
            HasCrushingBlow = tag.GetBool("HasCrushingBlow");
            OpenWoundsChance = tag.GetFloat("OpenWoundsChance");
            DeadlyStrikeChance = tag.GetFloat("DeadlyStrikeChance");
            CrushingBlowChance = tag.GetFloat("CrushingBlowChance");
            HasChanceToCast = tag.GetBool("HasChanceToCast");
            ProjID = tag.GetInt("ProjID");
            ChanceToCast = tag.GetFloat("ChanceToCast");
            CannotBeFrozen = tag.GetBool("CannotBeFrozen");
            LifeStolenPerHitPercentage = tag.GetFloat("LifeStolenPerHitPercentage");
            ManaStolenPerHitPercentage = tag.GetFloat("ManaStolenPerHitPercentage");
            oldDamage = tag.GetInt("oldDamage");
            ColdDamage = tag.GetInt("ColdDamage");
            FireDamage = tag.GetInt("FireDamage");
            PoisonDamage = tag.GetInt("PoisonDamage");
            LightningDamage = tag.GetInt("LightningDamage");
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)EnhancedDamage);
            writer.Write((byte)EnhancedDefense);
            writer.Write((byte)FasterCastRate);
            writer.Write((byte)FasterRunWalk);
            writer.Write((byte)Sockets);
            writer.Write((byte)MF);
            writer.Write(ColdTag);
            writer.Write(FireTag);
            writer.Write(PoisonTag);
            writer.Write(LightningTag);
            writer.Write((byte)ColdResistance);
            writer.Write((byte)FireResistance);
            writer.Write((byte)PoisonResistance);
            writer.Write((byte)LightningResistance);
            writer.Write((byte)PhysicalResistance);
            writer.Write(HasOpenWounds);
            writer.Write(HasDeadlyStrike);
            writer.Write(HasCrushingBlow);
            writer.Write((byte)OpenWoundsChance);
            writer.Write((byte)DeadlyStrikeChance);
            writer.Write((byte)CrushingBlowChance);
            writer.Write(HasChanceToCast);
            writer.Write((byte)ProjID);
            writer.Write((byte)ChanceToCast);
            writer.Write(CannotBeFrozen);
            writer.Write((byte)LifeStolenPerHitPercentage);
            writer.Write((byte)ManaStolenPerHitPercentage);
            writer.Write((byte)oldDamage);
            writer.Write((byte)item.damage);
            writer.Write((byte)ColdDamage);
            writer.Write((byte)FireDamage);
            writer.Write((byte)PoisonDamage);
            writer.Write((byte)LightningDamage);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            EnhancedDamage = reader.ReadByte();
            EnhancedDefense = reader.ReadByte();
            FasterCastRate = reader.ReadByte();
            FasterRunWalk = reader.ReadByte();
            Sockets = reader.ReadByte();
            MF = reader.ReadByte();
            ColdTag = reader.ReadBoolean();
            FireTag = reader.ReadBoolean();
            PoisonTag = reader.ReadBoolean();
            LightningTag = reader.ReadBoolean();
            ColdResistance = reader.ReadByte();
            FireResistance = reader.ReadByte();
            PoisonResistance = reader.ReadByte();
            LightningResistance = reader.ReadByte();
            PhysicalResistance = reader.ReadByte();
            HasOpenWounds = reader.ReadBoolean();
            HasCrushingBlow = reader.ReadBoolean();
            HasDeadlyStrike = reader.ReadBoolean();
            OpenWoundsChance = reader.ReadByte();
            CrushingBlowChance = reader.ReadByte();
            DeadlyStrikeChance = reader.ReadByte();
            HasChanceToCast = reader.ReadBoolean();
            ProjID = reader.ReadByte();
            ChanceToCast = reader.ReadByte();
            CannotBeFrozen = reader.ReadBoolean();
            LifeStolenPerHitPercentage = reader.ReadByte();
            ManaStolenPerHitPercentage = reader.ReadByte();
            oldDamage = reader.ReadByte();
            item.damage = reader.ReadByte();
            ColdDamage = reader.ReadByte();
            FireDamage = reader.ReadByte();
            PoisonDamage = reader.ReadByte();
            LightningDamage = reader.ReadByte();
        }
        #endregion
    }
}
