namespace Blossom.scripts.controllers.bugs;

public partial class FlyController : BaseBugController
{
    // Called when the node enters the scene tree for the first time.
    public override int MaxHealth => 5;
    public override int Damage => 2;
    public override float Cooldown => 1.2f;
    public override bool Flying => true;
}