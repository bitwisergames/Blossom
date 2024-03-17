namespace Blossom.scripts.controllers.bugs;

public partial class AsianBeetleController : BaseBugController
{
    // Called when the node enters the scene tree for the first time.
    public override int MaxHealth => 1;
    public override int Damage => 2;
    public override float Cooldown => 1.2f;
}