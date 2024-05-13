using Godot;
using System;

public partial class StartScreen : Control
{
	public GameManager gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = (GameManager)GetNode("/root/GameManager");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartRound()
	{
        GetTree().ChangeSceneToFile("res://Assets/Scenes/Main.tscn");
        gameManager.CallDeferred("StartRound");
	}

	public void QuitGame()
	{
		GetTree().Quit();
	}
}
