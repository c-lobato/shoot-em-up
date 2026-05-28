using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
    [Export] public int Speed;
    [Export] protected int Health;
    [Export] public int Points;
    [Export] public Sprite2D Sprite;
    [Export] public CollisionShape2D Hitbox;
    [Export] public AnimatedSprite2D Explosion;
    [Export] public AudioStreamPlayer2D Audio;
    [Signal] public delegate void OnDestroyedEnemyEventHandler(int points);

    private bool isDead = false;

    public override void _PhysicsProcess(double delta)
    {
        PhysicsUpdate();
        if(GlobalPosition.X < -100.0f)
        {
            QueueFree();
        }

        if(Health <= 0)
        {
            Die();
        }
    }
   
    public void TakeDamage()
    {
        Health -= 1;

        //sprite piscando pós tomar dano
        Sprite.Modulate = Colors.Red;
        Tween tween = CreateTween();
        tween.TweenProperty(Sprite, "modulate", Colors.White, 0.1f);
    }

    public abstract void PhysicsUpdate();
   public virtual void Die()
    {
        Speed = 0;
        if (isDead) return;
        isDead = true;  

        //sinal para creditar os pontos pós morte
        EmitSignal(SignalName.OnDestroyedEnemy, Points);

        if (Hitbox != null) Hitbox.SetDeferred("disabled", true);
        
        Sprite.Visible = false;

        if (Explosion != null)
        {
            Audio.Play();
            Explosion.Visible = true;
            Explosion.Play("death"); 
            Explosion.AnimationFinished += () => 
            {
                QueueFree();
            };
        }
        else
        {
            QueueFree();
        }
}
    public abstract void Attack();

}
