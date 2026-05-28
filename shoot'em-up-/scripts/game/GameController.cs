using Godot;
using System;
using System.Numerics;

public partial class GameController : Node
{
    [Export] public PackedScene WalkingEnemy;
    [Export] public PackedScene ShootingEnemy;
    [Export] public float SpawnInterval = 4.0f;
    [Export] public Label PlayerLabel;
    [Export] public Player player;
    [Export] public AudioStreamPlayer2D Audio;
    private Timer _spawnTimer;
    public int Score = 0;
    private float _screenWidth = 1152.0f;
    private float _screenHeight = 648.0f;
    private int lastCycle = 0;
    private float timePassed = 0f;
    private int currentMin = 0;
    private int currentSeg = 0;
    private bool bossSpawn = false;
    
    public override void _Ready()
    {
        //configurações da tela de jogo 
        _spawnTimer = new Timer();
        AddChild(_spawnTimer);
        _spawnTimer.WaitTime = SpawnInterval;
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += OnSpawnTimerTimeout;
        _spawnTimer.Start();
        Audio.Play();

        //verificação do sinal de morte do jogador
        player.OnPlayerDied += GameOver;
        
        //reset de velocidade do parallax 
        Parallax2D background = GetTree().Root.GetNode<Parallax2D>("Background");
        if (background != null)
        {
            background.Autoscroll = new Godot.Vector2(-24f, 0f); // Velocidade mais calma para o menu
        }
    }    

    //o timer obedece a passagem de 1 currentMin para alterar sua duração
    public override void _PhysicsProcess(double delta)
    {
        timePassed+=(float)delta;

        //tempo para label da hud
        currentMin = Mathf.FloorToInt(timePassed / 60f);
        currentSeg = Mathf.FloorToInt(timePassed % 60f);

        //cria os ciclos de 30 segundos do jogo (cada 30s vai diminuir o tempo de spawn e aumentar a velicidade do bg)
        int currentCicle = Mathf.FloorToInt(timePassed / 30f);
        
        if (currentCicle > lastCycle)
        {
            //salva o ciclo de 30 segundos do jogo
            lastCycle = currentCicle;
            double newSpawnTime = _spawnTimer.WaitTime - 0.3f;
            
            //verificação de segurança (o spawn dos inimigos não podem passar de 3 s de intervalo)
            if(newSpawnTime <= 1.5f)
            {
                newSpawnTime = 1.5f;
            }

            _spawnTimer.WaitTime = newSpawnTime;

            //velocidade do parallax global
            Parallax2D background = GetTree().Root.GetNode<Parallax2D>("Background");
            if(background != null)
            {
                //adiciona 30 frames na contagem, pra uma sensação de aumento de velocidade
                float velX = background.Autoscroll.X - 30f;
                //limite de velocidade do parallax 
                if (velX <= -200f)
                {
                    velX = -200f;
                }
                background.Autoscroll = new Godot.Vector2(velX, background.Autoscroll.Y);

            }

            GD.Print($"novo timer: {_spawnTimer}");
        }
        PlayerLabel.Text = $"HP: {player.Health}\nTime: {currentMin:D2}:{currentSeg:D2}\nScore: {Score}";
    }

    private void GameOver()
    {   
        //para a musica e o spawn de inimigos
        Audio.Stop();
        _spawnTimer.Stop();
        
        //desativação da física do grupo "enemys" que contem os inimigos e seu projétil
        var enemys = GetTree().GetNodesInGroup("enemys");
        foreach (Node enemy in enemys){
            enemy.ProcessMode = ProcessModeEnum.Disabled;
        }
        
        //timer de espera da animação do jogador e chamada da tela de game over (estilo mario)
        var delayTimer = GetTree().CreateTimer(1.5f,true,true);
        delayTimer.Timeout += () =>
        {
            GD.Print("explosao acabou, chamando a tela game over");
            SetupPlayerStatus();
            GetTree().ChangeSceneToFile("res://scenes/game/GameOverScreen.tscn");
        }; 
    }

    private void SetupPlayerStatus()
    {
        var global = GetNode<GameData>("/root/GameData");
        global.FinalScore = Score;
        global.FinalTime = $"{currentMin:D2}:{currentSeg:D2}";
    }

    //spawn de inimigos, 3 diferentes grupos que spawnam respeitando o timer
    private void OnSpawnTimerTimeout()
    {
        if (WalkingEnemy == null || ShootingEnemy == null) return;

        // sorteia um número de 0 a 2 para decidir o padrão da rodada
        int padraoSorteado = GD.RandRange(0, 1);

        //inimigo padrão
        if (padraoSorteado == 0)
        {   
            //escolhe se vai ser um grupo de 4 ou 5 inimigos
            int enemyGroup = (int)GD.RandRange(4,5);
            //limites de spawn dentro da tela (superior / inferior) para evitar cortar as naves
            float supLimit = 35f;
            float infLimit = 610f;
            float totalHeight = infLimit-supLimit;

            for (int i=0; i<enemyGroup; i++)
            {
                //cálculo da linha de spawn dos inimigos, pra evitar sobreposição     
                float lineSize = totalHeight / enemyGroup;   
                float superiorSpawnLine =  supLimit + (i*lineSize);
                float inferiorSpawnLine = superiorSpawnLine + lineSize;

                float spawnY = (float)GD.RandRange(superiorSpawnLine-20f,inferiorSpawnLine-30f);
                float spawnX = (float)GD.RandRange(1250,1350);
                SpawnEnemy(WalkingEnemy, spawnX, spawnY);
            }
            
        }
        else    //spawna inimigos atiradores em duplas ou trios
        {
            int enemyGroup = (int)GD.RandRange(2,3);

            float supLimit =40f;
            float infLimit = 400f;
            float totalHeight = infLimit-supLimit;

            for (int i=0; i<enemyGroup; i++)
            {
                //cálculo da linha de spawn dos inimigos, pra evitar sobreposição     
                float lineSize = totalHeight / enemyGroup;   
                float superiorSpawnLine =  supLimit + (i*lineSize);
                float inferiorSpawnLine = superiorSpawnLine + lineSize;

                float spawnX = (float)GD.RandRange(1250,1350);
                float spawnY = (float)GD.RandRange(superiorSpawnLine-20f,inferiorSpawnLine-30f);
                SpawnEnemy(ShootingEnemy, spawnX, spawnY);
            }       
        }
    }

    //instancia dos inimigos para o OnSpawnTimerTimeout
    private void SpawnEnemy(PackedScene scene, float x, float y)
    {
        Enemy enemy = scene.Instantiate<Enemy>();
        enemy.GlobalPosition = new Godot.Vector2(x, y);
        enemy.OnDestroyedEnemy += SomarPontos;
        AddChild(enemy); 
    }

    private void SomarPontos(int pontosDoInimigo)
    {
        Score += pontosDoInimigo;
    }

}
