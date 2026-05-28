using Godot;
using System;

public partial class Menu : Control
{
    [Export] public Label Title;
    [Export] public Button Start;
    [Export] public Button Quit;
    [Export] public AnimationPlayer Anim;
    [Export] public Sprite2D Sprite;

    public override void _Ready()
    {
        //ConfigureUI();
        Anim.Play("title_anim");
        Sprite.Visible = false;

        if (Start != null) Start.Pressed += OnStartButtonPressed;
        if (Quit != null) Quit.Pressed += OnQuitButtonPressed;

        //reset de velocidade do parallax 
        Parallax2D background = GetTree().Root.GetNode<Parallax2D>("Background");
        if (background != null)
        {
            background.Autoscroll = new Godot.Vector2(-24f, 0f); // Velocidade mais calma para o menu
        }
    }


    private void OnStartButtonPressed()
    {
        Sprite.Visible = true;
        Anim.Play("start_transition");

        Anim.AnimationFinished += OnTransitionFinished;
    }

    private void OnTransitionFinished(StringName name)
    {
        if (name == "start_transition")
        {
            //troca para a cena do jogo de fato
            GetTree().ChangeSceneToFile("res://scenes/game/World.tscn");
        }
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

}
