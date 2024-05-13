using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public GameManager gameManager;
    public Player Player;
    public Area2D Area; 
    public Vector2 ScreenSize = new(320, 280);
    public AnimatedSprite2D enemySprite;
    public float facing;
    private Vector2 velocity = new();
    public int score = 1000;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("/root/GameManager");
        enemySprite = GetNode<AnimatedSprite2D>("EnemySprite");
        Player = (Player)GetNode<CharacterBody2D>("/root/Main/Player");
                                                                       
        // Set the initial velocity
        bool isPositive = (GD.RandRange(0, 1) > 0.5f); // Randomly choose positive or negative
        float x = isPositive ? GD.RandRange(20, 30) : GD.RandRange(-30, -20);
        float y = isPositive ? GD.RandRange(20, 30) : GD.RandRange(-30, -20);
        velocity = new Vector2(x, y);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Player == null) return;
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;
            velocity.X += 5 * (float)delta;

        var collisionInfo = MoveAndCollide(velocity * (float)delta);
        if (collisionInfo != null)
        {
            GD.Print("Collide");
            velocity = velocity.Bounce(collisionInfo.GetNormal());
        }
        enemySprite.Play("Walk");
        if (Position.X > ScreenSize.X)
        {
            Position = new Vector2(0, Position.Y);
        }
        else if (Position.X < 0)
        {
            Position = new Vector2(ScreenSize.X, Position.Y);
        }
        if (velocity.X > 0)
        {
            FlipEnemy(1);
        }
        else
        {
            FlipEnemy(-1);
        }
        if(Position.Y > ScreenSize.Y)
        {
            QueueFree();
        }
    }

    public void FlipEnemy(float dir)
    {
        if (dir != facing)
        {
            if (dir == 1f)
            {
                GlobalTransform = new Transform2D(new Vector2(1, 0), new Vector2(0, 1), new Vector2(Position.X, Position.Y));
            }
            else if (dir == -1f)
            {
                GlobalTransform = new Transform2D(new Vector2(-1, 0), new Vector2(0, 1), new Vector2(Position.X, Position.Y));
            }
            GD.Print(Scale);
            facing = dir;
        }
    }

    public void KillEnemy()
    {
        gameManager.UpdateScore(1000);
    }

    public void ReportCollision(Node2D node)
    {
        if(node.Name == "Player")
        {
            if(node.Position.Y > Position.Y)
            {
                node.QueueFree();
            }
        }
    }
}
