using Godot;
using System;

public partial class GameManager : Node2D
{
	public int score = 0;
	public Node2D mainScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartRound()
	{
		score = 0;
    }
	public void UpdateScore(int addThis)
	{
		score += addThis;
		mainScene.GetNode<RichTextLabel>("Score").Text = score.ToString();
	}
}
