using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
    [Export] protected int Speed;
    [Export] protected int Health;

    public override void _Ready()
    {
        SpawnMethod();
    }
    public abstract void SpawnMethod();
    public override void _PhysicsProcess(double delta)
    {
        PhysicsUpdate();
    }
    public abstract void PhysicsUpdate();
    public abstract void Die();
    public abstract void Attack();

}
