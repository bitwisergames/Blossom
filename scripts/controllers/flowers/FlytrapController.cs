namespace Blossom.scripts.controllers.flowers;

public partial class FlytrapController : BaseFlowerController
{
    public override int[] Income => [1, 1, 2, 2];
    public override int[] UpgradeCost => [3, 6, 9];
    public override int[] ScaledDamage => [10, 10, 10, 10];
    public override float Cooldown => 30f;
    public override bool Ranged => false;
    public override bool CanTargetMultiple => true;
}