using Godot;
using System;

public partial class EnemyProjectile : Area2D
{
    [Export] public float Speed = 360.0f;
    [Export] AnimationPlayer Anim;

    public override void _Ready()
    {
        Anim.Play("enemy_projectile");
    }


    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Vector2.Right * -Speed * (float)delta;
    }

    public void OnEnemyProjectileHitPlayer(Node2D body)
    {
        if(body is Player player)
        {   
            player.TakeDamage();
        }
    }
}

