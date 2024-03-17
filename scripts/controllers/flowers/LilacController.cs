namespace Blossom.scripts.controllers.flowers;

public partial class LilacController : BaseFlowerController
{
    public override int[] Income => [1, 2, 3, 4];
    public override int[] Upgrade => [4, 8, 12];
    public override bool Passive => true;
}