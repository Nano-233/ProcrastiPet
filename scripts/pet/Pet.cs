using Godot;

public partial class Pet : Node2D
{
	[Export] public string PetName = "嘟嘟";
	[Export] public int Level = 1;
	[Export] public int XP = 0;
	[Export] public int XPNeeded = 100;

	private Label _nameLabel;
	private Label _levelLabel;
	private TextureProgressBar _xpBar;
	private AnimatedSprite2D _sprite;
	private Button _feedButton;
	private Timer _animationTimer;

	public override void _Ready()
	{
		// Get references to UI elements
		_nameLabel = GetNode<Label>("PetName");
		_levelLabel = GetNode<Label>("PetLevel");
		_xpBar = GetNode<TextureProgressBar>("TextureProgressBar");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_feedButton = GetNode<Button>("FeedButton");

		// Create a timer for animation reset
		_animationTimer = new Timer();
		_animationTimer.WaitTime = 1.5f; // Set to 1.5 seconds
		_animationTimer.OneShot = true;
		AddChild(_animationTimer);
		_animationTimer.Timeout += OnAnimationTimerTimeout;

		// Connect button signal
		_feedButton.Pressed += OnFeedButtonPressed;

		UpdateUI();
	}

	private void OnFeedButtonPressed()
	{
		GainXP(10); // Give 10 XP when feeding
		GD.Print("Pet fed! XP increased.");

		// Change animation to "eating"
		_sprite.Play("eating");

		// Start the timer to switch back to "idle"
		_animationTimer.Start();
	}

	private void OnAnimationTimerTimeout()
	{
		_sprite.Play("idle"); // Return to idle after timer runs out
	}

	public void GainXP(int amount)
	{
		XP += amount;
		if (XP >= XPNeeded)
		{
			LevelUp();
		}
		UpdateUI();
	}

	private void LevelUp()
	{
		XP -= XPNeeded;  // Carry over excess XP
		Level++;
		XPNeeded = (int)(XPNeeded * 1.2);  // Increase XP requirement
		UpdateUI();
	}

	private void UpdateUI()
	{
		_nameLabel.Text = PetName;
		_levelLabel.Text = "Level " + Level.ToString();
		_xpBar.Value = XP;
		_xpBar.MaxValue = XPNeeded;
	}
}
