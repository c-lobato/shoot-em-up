using Godot;
using System;
using System.Reflection.Metadata;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed = 400;
    [Export] public int Health = 5;
    [Export] public PackedScene projectile;

    private bool isDead = false;
    private bool canShoot = true;

    public override void _PhysicsProcess(double delta)
    {
        if (isDead == true) return;

        HandleMovement();
        HandleShooting();

    }

    public void HandleMovement()
    {
        Vector2 dir = Input.GetVector("left","right","up","down");
        Velocity = dir * Speed;
        MoveAndSlide();
    }

    public void HandleShooting()
    {

        if (Input.IsActionJustPressed("shoot") && canShoot)
        {
            Projectile new_projectile = projectile.Instantiate<Projectile>();
            new_projectile.GlobalPosition = GlobalPosition;
            GetTree().Root.AddChild(new_projectile);
            
        }
        
    }

    

}
