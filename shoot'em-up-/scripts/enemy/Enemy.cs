using Godot;
using System;

public abstract partial class Enemy : CharacterBody2D
{
    [Export] protected int Speed;
    [Export] protected int Health;

}
