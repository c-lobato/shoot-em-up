using Godot;
using System;

public partial class ShootingEnemy : Enemy
{
    private float _timePassed = 0.0f;
    [Export] public Timer ShootingCooldown;
    [Export] public float Frequency = 5.0f; 
    [Export] public float Amplitude = 300.0f; 
    public override void SpawnMethod(){}
    public override void PhysicsUpdate()
    {
       _timePassed += (float)GetProcessDeltaTime();
       float velocityY = Mathf.Sin(_timePassed * Frequency) * Amplitude;
       Velocity = new Godot.Vector2(-Speed,velocityY); 
       MoveAndSlide();
    }
    public override void Die(){}
    public override void Attack()
    {
        
    }
}
