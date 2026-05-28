using Godot;
using System;
using System.Numerics;

public partial class WalkingEnemy : Enemy
{
    public override void PhysicsUpdate()
    {
       //movimentação em X
       Velocity = new Godot.Vector2(-Speed,0); 
       MoveAndSlide();
    }

    public override void Attack(){}

}
