using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities;


public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth = 4;
    private int currentHealth;
    private int currentScore = 0;

    public GUI() : base("pacman")
    {
        scoreText = new Text();
        sprite.TextureRect = new IntRect(4 * 18, 2 * 18, 18, 18);
    }

    public override void Create(Scene scene)
    {
        base.Create(scene);
        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.DisplayedString = "Score";
        scoreText.CharacterSize = 60;
        scoreText.Scale = new Vector2f(0.5f, 0.5f);
        scoreText.FillColor = Color.Black;
        currentHealth = maxHealth;
        scene.Events.LoseHealth += OnLoseHealth;
        scene.Events.GainScore += OnScoreGained;
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.LoseHealth -= OnLoseHealth;
        scene.Events.GainScore -= OnScoreGained;
    }

    private void OnLoseHealth(Scene scene, int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DontDestroyOnLoad = false;
            scene.Loader.Reload();
        }
    }

    private void OnScoreGained(Scene scene, int amount)
    {
        currentScore += amount;

        // Reset when all coins are picked up
        if (!scene.FindByType<Coin>(out _))
        {
            DontDestroyOnLoad = true;
            scene.Loader.Reload();
        }
    }

    public override void Render(RenderTarget target)
    {
        sprite.Position = new Vector2f(36, 396);
        for (int i = 0; i < maxHealth; i++)
        {
            sprite.TextureRect = i < currentHealth
            ? new IntRect(72, 36, 18, 18)
            : new IntRect(72, 0, 18, 18);
            base.Render(target);
            sprite.Position += new Vector2f(18, 0);
        }
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(414 - scoreText.GetGlobalBounds().Width, 396);
        target.Draw(scoreText);
    }
}
