using Terraria.ModLoader;

public class RollRange : GlobalItem {
    public override bool InstancePerEntity => true;
    public int Min;
    public int Max;
    public RollRange(int min, int max) {
        Min = min;
        Max = max;
    }
}