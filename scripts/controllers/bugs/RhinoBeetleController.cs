using Blossom.scripts.interfaces;

namespace Blossom.scripts.controllers.bugs;

public partial class RhinoBeetleController : BaseBugController
{
    public override int MaxHealth => 20;
    public override int Damage => 5;
    public override float Cooldown => 1.2f;
}