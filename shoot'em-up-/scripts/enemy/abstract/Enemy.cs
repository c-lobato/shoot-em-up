using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
    [Export] protected int Speed;
    [Export] protected int Health;
    [Export] public int Points;

    public override void _PhysicsProcess(double delta)
    {
        PhysicsUpdate();
        if(GlobalPosition.X < -100.0f)
        {
            QueueFree();
        }
    }

    public void SetDificuldade(float bonusVelocidade)
    {
        
    }

    public abstract void PhysicsUpdate();
    public abstract void Die();
    public abstract void Attack();

}
