using Godot;
using System;
using System.Collections;

public partial class ShootingEnemy : Enemy
{
    private float _timePassed = 0.0f;
    [Export] public Timer ShootingCooldown;
    [Export] public PackedScene enemy_projectile;
    [Export] public float Frequency = 5.0f; 
    [Export] public float Amplitude = 300.0f;

    public override void _Ready()
    {
        ShootingCooldown.OneShot = false;
        ShootingCooldown.Timeout += OnShootTimeout;
        ShootingCooldown.Start();
    }

    private void OnShootTimeout()
    {
        Attack();
    }

    public override void PhysicsUpdate()
    {
        _timePassed += (float)GetProcessDeltaTime();
        float velocityY = Mathf.Sin(_timePassed * Frequency) * Amplitude;
        Velocity = new Godot.Vector2(-Speed,velocityY); 
        MoveAndSlide();

        //verificação de morte
        if(Health == 0)
        {
            Amplitude = 0;
                Die();
        }
    }

    public override void Attack()
    {
        //INSTANCIA UM ENEMYPROJECTILE, DA MESMA FORMA QUE O PLAYER INSTANCIA O TIRO DELE
        EnemyProjectile new_projectile = enemy_projectile.Instantiate<EnemyProjectile>();
        new_projectile.GlobalPosition = GlobalPosition;
        GetTree().Root.AddChild(new_projectile);
    }
}
