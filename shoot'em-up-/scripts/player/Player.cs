using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] protected int Speed = 400;
    [Export] protected int Health = 5;
    
    protected int Score;
     
    
}
