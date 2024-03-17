namespace Blossom.scripts.controllers.flowers;

public partial class LilacController : BaseFlowerController
{
    public override int[] Income => [1, 2, 3, 4];
    public override int[] UpgradeCost => [4, 8, 12];
    public override int[] ScaledDamage => [2, 3, 4, 6];
    public override bool Passive => true;
}