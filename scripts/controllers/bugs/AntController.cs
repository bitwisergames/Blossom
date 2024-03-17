namespace Blossom.scripts.controllers.bugs;

public partial class AntController : BaseBugController
{
    public override int MaxHealth => 2;
    public override int Damage => 1;
    public override float Cooldown => 1.2f;
}