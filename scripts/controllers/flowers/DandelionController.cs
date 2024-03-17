namespace Blossom.scripts.controllers.flowers;

public partial class DandelionController : BaseFlowerController
{
    public override int[] Income => [1, 1, 2, 2];
    public override int[] UpgradeCost => [3, 6, 9];
    public override int[] ScaledDamage => [2, 3, 4, 6];
    public override float Cooldown => 1.5f;
    public override bool Ranged => false;
    public override bool CanTargetMultiple => true;
}