namespace Blossom.scripts.controllers.bugs;

public partial class SpiderController : BaseBugController
{
    public override int MaxHealth => 10;
    public override int Damage => 3;
    public override float Cooldown => 1.0f;
    public override float Speed => 5000f;
}