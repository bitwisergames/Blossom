using Godot;

namespace Blossom.scripts.controllers.flowers;

public partial class GladiolusFlowerController : BaseFlowerController
{
    public override int[] Income => [1, 1, 2, 3];
    public override int[] Upgrade => [3, 6, 9];
    public override int Damage => 1;
    public override float Cooldown => 1f;

    public override void Attack(Node2D target)
    {
        /*
        (target as IDamagable)?.InflictDamage(Damage);
        Need to spawn projectile here
        */
    }
}