using SFML.Graphics;

namespace Pacman.Entities;


public class Ghost : Actor
{
    private float frozenTimer = 0f;

    private void OnCandyEaten(Scene scene, int amount) => frozenTimer = 5f;

    public override void Create(Scene scene)
    {
        direction = -1;
        speed = 100f;
        moving = true;
        base.Create(scene);
        scene.Events.CandyEaten += OnCandyEaten;
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.CandyEaten -= OnCandyEaten;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        base.Update(scene, deltaTime);
        frozenTimer = MathF.Max(frozenTimer - deltaTime, 0f);
    }

    public override void Render(RenderTarget target)
    {
        sprite.TextureRect = frozenTimer <= 0f
        ? new IntRect(36, 0, 18, 18)
        : new IntRect(36, 18, 18, 18);
        base.Render(target);
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if (other is Pacman)
        {
            if (frozenTimer <= 0f)
                scene.Events.PublishLoseHealth();
            else
                scene.Events.PublishGainScore(500);
            Reset();
        }
    }

    protected override int PickDirection(Scene scene)
    {
        List<int> validMoves = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if ((i + 2) % 4 == direction) continue;
            if (IsFree(scene, i)) validMoves.Add(i);
        }
        int r = new Random().Next(0, validMoves.Count);
        return validMoves[r];
    }
}
