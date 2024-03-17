namespace Blossom.scripts.controllers.flowers;

public partial class DandelionController : BaseFlowerController
{
    public override int[] Income => [1, 1, 2, 3];
    public override int[] Upgrade => [3, 6, 9];
    public override int Damage => 2;
    public override float Cooldown => 1.5f;
    public override bool Ranged => false;
    public override bool CanTargetMultiple => true;
}