using Godot;
using System;

public partial class GameOver : Control
{
    [Export] public Button Restart;
    [Export] public Button Menu;
    [Export] public Button Quit;
    [Export] public Label PlayerInfo;
    [Export] public AudioStreamPlayer2D Audio;

    public override void _Ready()
    {
        Audio.Play();
        ConfigureStatusLabel();

        if(Restart != null) Restart.Pressed += OnRestartButtonPressed;
        if(Menu != null) Menu.Pressed += OnMenuButtonPressed;
        if(Quit != null) Quit.Pressed += OnQuitButtonPressed;
    }

    private void ConfigureStatusLabel()
    {
        var global = GetNode<GameData>("/root/GameData");
        PlayerInfo.Text = $"TIME: {global.FinalTime}      SCORE: {global.FinalScore}";
    }

    private void OnRestartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game/World.tscn");
    }

    private void OnMenuButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game/start_menu.tscn");

    }
    
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }


}
