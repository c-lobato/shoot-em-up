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
    [Export] public AnimatedSprite2D Anim;
    [Signal] public delegate void OnPlayerDiedEventHandler();

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

        //verificação de morte do player caso esteja vivo + lançamento do sinal OnPlayerHitFatal do controlador
        if (Health <= 0 && !isDead)
        {
            PlayerDeath();
            return;
        }

        HandleMovement();
        HandleShooting();

        if (isInvincible == true)
        {
            Sprite.Visible = (Time.GetTicksMsec() / 100) % 2 == 0;
        }

    }

    //dano de colisao direta com outra nave
    private void OnHitboxBodyEntered(Node2D body)
    {
        if (body is Enemy && !isInvincible)
        {
            TakeDamage();
            ((Enemy)body).Die();
        }
        
    }

    //metodo de dano do player
    public void TakeDamage()
    {
        //toma 1 de dano
        Health -=1;
        //inicia o iframe 
        isInvincible = true;
        iFrameTimer.Start(); 
    }

    //morte do player + sinal de morte (OnPlayerDied)
    public void PlayerDeath()
    {
       if (isDead) return;
       isDead = true;
       Speed = 0;
       Sprite.Visible = false;

        //alterando o process mode do player e seus filhos
       this.ProcessMode = ProcessModeEnum.Always;

        //toca a animação
       if(Anim != null)
        {
            Anim.Frame = 0;
            Anim.Play("death");
        }

       //sinal de morte
       EmitSignal(SignalName.OnPlayerDied);
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
