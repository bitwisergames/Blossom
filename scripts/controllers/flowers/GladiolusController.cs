using Godot;

namespace Blossom.scripts.controllers.flowers;

public partial class GladiolusController : BaseFlowerController
{
    public override int[] Income => [1, 1, 2, 3];
    public override int[] UpgradeCost => [3, 6, 9];
    public override int[] ScaledDamage => [2, 3, 4, 6];
    public override float Cooldown => 1f;
}