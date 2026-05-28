using Godot;
using System;

public partial class EnemyProjectile : Area2D
{
    [Export] public float Speed = 400.0f;

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Vector2.Right * -Speed * (float)delta;
    }

    public void OnEnemyProjectileHitEnemy(Node2D body)
    {
        if(body is Player player)
        {   
            player.TakeDamage();
        }
    }
}

