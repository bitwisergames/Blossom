using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.flowers;

public partial class FirespikeController : BaseFlowerController
{
    public override int[] Income => [1, 1, 1, 2];
    public override int[] UpgradeCost => [3, 6, 9];
    public override int[] ScaledDamage => [1, 1, 2, 3];
    public override float Cooldown => 2f;

    public override void Attack(Node2D target)
    {
        if (CanTargetMultiple) return;
        if (!Ranged && (target.GetParent() as IMoveable)!.Flying) return;

        // Spawn Projectile
        var projectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn").Instantiate() as Node2D;
        AddChild(projectileScene);

        var projectile = projectileScene as ProjectileController;
        projectile?.Setup(target, 1, 5000f, true, 100f);
        animPlayer.Play(target.GlobalPosition.X > GlobalPosition.X
            ? "FlowerAnimations/AttackRight"
            : "FlowerAnimations/AttackLeft");
        animPlayer.Queue("FlowerAnimations/Idle");
    }
}