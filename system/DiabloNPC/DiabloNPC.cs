using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace UnlimitedMod.system.DiabloItem
{
    public class DiabloNPC : GlobalNPC
    {
        // Setting enemy resistances
        public float NPC_ColdResistance, NPC_FireResistance, NPC_PoisonResistance, NPC_LightningResistance;
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults() {
            
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.GetGlobalItem<DiabloItem>().ColdTag) {
                npc.defense = (int)(npc.defense * Math.Floor(NPC_ColdResistance));
            }
            if (item.GetGlobalItem<DiabloItem>().FireTag) {
                npc.defense = (int)(npc.defense * Math.Floor(NPC_FireResistance));
            }
            if (item.GetGlobalItem<DiabloItem>().PoisonTag) {
                npc.defense = (int)(npc.defense * Math.Floor(NPC_PoisonResistance));
            }
            if (item.GetGlobalItem<DiabloItem>().LightningTag) {
                npc.defense = (int)(npc.defense * Math.Floor(NPC_LightningResistance));
            }
        }
        public override void OnKill(NPC npc)
        {
            Main.NewText($"Cold RES: {Math.Round(NPC_ColdResistance*100)}. Fire RES: {Math.Round(NPC_FireResistance*100)}");
            Main.NewText($"LIGHT RES: {Math.Round(NPC_LightningResistance*100)}, Poison RES: {Math.Round(NPC_PoisonResistance*100)}");
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            NPC_ColdResistance = Main.rand.NextFloat(0, 0.75f);
            NPC_FireResistance = Main.rand.NextFloat(0, 0.75f);
            NPC_PoisonResistance = Main.rand.NextFloat(0, 0.75f);
            NPC_LightningResistance = Main.rand.NextFloat(0, 0.75f);
        }


    }
}

    
