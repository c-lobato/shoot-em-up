using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public float Speed = 600.0f;
    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Vector2.Right * Speed * (float)delta;
    }

}
