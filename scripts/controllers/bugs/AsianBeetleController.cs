namespace Blossom.scripts.controllers.bugs;

public partial class AsianBeetleController : BaseBugController
{
    // Called when the node enters the scene tree for the first time.
    public override int MaxHealth => 3;
    public override int Damage => 2;
    public override float Cooldown => 1.5f;
    public override bool Flying => true;
    public override float Speed => 5500f;
}