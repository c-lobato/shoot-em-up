using Godot;
using System;

public partial class GameController : Node
{
    [Export] public PackedScene WalkingEnemy;
    [Export] public PackedScene ShootingEnemy;
    [Export] public float SpawnInterval = 3.0f;

    private Timer _spawnTimer;
    public int Score = 0;
    private float _screenWidth = 1152.0f;
    private float _screenHeight = 648.0f;
     

    public override void _Ready()
    {
        _spawnTimer = new Timer();
        AddChild(_spawnTimer);
        _spawnTimer.WaitTime = SpawnInterval;
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += OnSpawnTimerTimeout;
        _spawnTimer.Start();
    }

    private void OnSpawnTimerTimeout()
    {
        if (WalkingEnemy == null || ShootingEnemy == null) return;

        // sorteia um número de 0 a 2 para decidir o padrão da rodada
        int padraoSorteado = GD.RandRange(0, 2);

        float spawnX = 1250.0f; 

        if (padraoSorteado == 0)
        {
            float[] posicoesY = { 150f, 324f, 500f };
            foreach (float y in posicoesY)
            {
                SpawnEnemy(WalkingEnemy, spawnX, y);
                GD.Print("padrao 0 spawnado");
            }
        }
        else if (padraoSorteado == 1)
        {
            SpawnEnemy(ShootingEnemy, spawnX, 100f);
            SpawnEnemy(ShootingEnemy, spawnX, 300f);
            GD.Print("padrao 1 spawnado");
        }
        else
        {
            float yAleatorio = (float)GD.RandRange(80f, 350f);
            PackedScene inimigoSorteado = GD.Randf() > 0.5f ? WalkingEnemy : ShootingEnemy;
            SpawnEnemy(inimigoSorteado, spawnX, yAleatorio);
            GD.Print("padrao 2 spawnado");
        }
    }

    private void SpawnEnemy(PackedScene scene, float x, float y)
    {
        Enemy enemy = scene.Instantiate<Enemy>();
        enemy.GlobalPosition = new Vector2(x, y);
        AddChild(enemy);    
    }

    private void CalculateScore()
    {
        //Score += Enemy.Points;
    }

}
