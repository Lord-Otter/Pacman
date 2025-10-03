namespace Pacman.Managers;


public class EventManager
{
    private int scoreGained = 0;
    private int healthLost = 0;
    private int eatenCandy = 0;

    // Events
    public event ValueChangedEvent? GainScore;
    public event ValueChangedEvent? LoseHealth;
    public event ValueChangedEvent? CandyEaten;

    // Event publishers
    public void PublishGainScore(int amount) => scoreGained += amount;
    public void PublishLoseHealth(int amount = 1) => healthLost += amount;
    public void PublishCandyEaten() => eatenCandy++;

    public void DispatchEvents(Scene scene)
    {
        if (scoreGained != 0)
        {
            GainScore?.Invoke(scene, scoreGained);
            scoreGained = 0;
        }

        if (healthLost != 0)
        {
            LoseHealth?.Invoke(scene, healthLost);
            healthLost = 0;
        }

        if (eatenCandy != 0)
        {
            CandyEaten?.Invoke(scene, eatenCandy);
            eatenCandy = 0;
        }
    }
}
