using Terraria;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod.system {
    public class MobInteraction : GlobalNPC {

        public override void OnKill(NPC npc)
        {
            // XP Gain is based off of the killed NPC's max life
            Level.SetXPGain(npc.lifeMax);
            // DEBUG
            Main.NewText($"Você ganhou um total de {Level.xp} XP e está level {Level.level}");
            Main.NewText($"Rolled {ProbabilitySystem.CalculateBaseProbabilities()}");
            
        }

    }
}