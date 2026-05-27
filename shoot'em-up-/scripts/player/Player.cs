using Godot;
using System;
using System.Reflection.Metadata;

public partial class Player : CharacterBody2D
{
    [Export] public int Speed;
    [Export] public int Health;
    [Export] public PackedScene projectile;
    [Export] public Sprite2D Sprite;
    [Export] public Area2D Hitbox;
    [Export] public Timer iFrameTimer;
    

    private bool isDead = false;
    private bool isInvincible = false;
    
    private bool canShoot = true;

    public override void _Ready()
    {
        Hitbox.BodyEntered += OnHitboxBodyEntered;
        iFrameTimer.OneShot = true;
        iFrameTimer.Timeout += () => { isInvincible = false; Sprite.Visible = true; };
    }


    public override void _PhysicsProcess(double delta)
    {
        if (isDead == true) return;

        HandleMovement();
        HandleShooting();

        if (isInvincible == true)
        {
            Sprite.Visible = (Time.GetTicksMsec() / 100) % 2 == 0;
        }

    }

    private void OnHitboxBodyEntered(Node2D body)
    {
        if (body is Enemy && !isInvincible)
        {
            isInvincible = true;
            iFrameTimer.Start();

            Health -= 1;

            ((Enemy)body).Die();
        }
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
