using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public GameManager gameManager;
    public Vector2 ScreenSize = new(320, 280);

    public const float Speed = 100.0f;
    public const float JumpVelocity = -300.0f;

    public float facing = 1f;
    public AnimatedSprite2D playerSprite;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle() * 60;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("/root/GameManager");
        playerSprite = GetNode<AnimatedSprite2D>("PlayerSprite");
        gameManager.mainScene = GetNode<Node2D>("/root/Main");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            playerSprite.Play("Walk");
            FlipPlayer(direction.X);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            playerSprite.Stop();
            playerSprite.Frame = 0;
        }

        Velocity = velocity;
        MoveAndSlide();
        if(Position.X > ScreenSize.X)
        {
            Position = new Vector2(0, Position.Y);
        }   
        else if(Position.X < 0)
        {
            Position = new Vector2(ScreenSize.X, Position.Y);
        }
    }

    public void FlipPlayer(float dir)
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

    public void ReportCollision(Node2D node)
    {
        GD.Print(node.Name);
        if (node.Name.ToString().Contains("Enemy"))
        {
            if (node.Position.Y > Position.Y)
            {
                Enemy enemy = node as Enemy;
                gameManager.UpdateScore(enemy.score);
                node.QueueFree();
            }
        }
    }
}
