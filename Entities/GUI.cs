using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pacman.Entities;


public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth = 4;
    private int currentHealth;
    private int currentScore = 0;
    private int highScore;
    private bool isGameOver = false;

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
            DontDestroyOnLoad = true;
            scene.Clear();
            UpdateHighScore();
            isGameOver = true;
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

    private void UpdateHighScore()
    {
        string highScoreFile = "assets/highscore.txt";

        if (File.Exists(highScoreFile))
        {
            string content = File.ReadAllText(highScoreFile);
            int.TryParse(content, out highScore);
        }

        if (currentScore > highScore)
        {
            highScore = currentScore;

            File.WriteAllText(highScoreFile, highScore.ToString());
        }
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (!isGameOver) return;

        if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
        {
            isGameOver = false;
            DontDestroyOnLoad = false;
            scene.Loader.Reload();
        }
    }

    public override void Render(RenderTarget target)
    {
        if (isGameOver)
        {
            // Draw game over
            scoreText.CharacterSize = 100;
            scoreText.DisplayedString = "GAME OVER";
            scoreText.Position = new Vector2f((18 * 23 - scoreText.GetGlobalBounds().Width) / 2, 150);
            target.Draw(scoreText);

            scoreText.CharacterSize = 35;
            scoreText.DisplayedString = "Press 'SPACE' to try again!";
            scoreText.Position = new Vector2f((18 * 23 - scoreText.GetGlobalBounds().Width) / 2, 400);
            target.Draw(scoreText);

            // Draw highscore
            scoreText.CharacterSize = 60;
            scoreText.DisplayedString = $"HighScore: {highScore}";
            scoreText.Position = new Vector2f((18 * 23 - scoreText.GetGlobalBounds().Width) / 2, 250);
            target.Draw(scoreText);

            // Draw score
            scoreText.DisplayedString = $"Score: {currentScore}";
            scoreText.Position = new Vector2f((18 * 23 - scoreText.GetGlobalBounds().Width) / 2, 300);
            target.Draw(scoreText); 
            return;
        }

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
