using SFML.Graphics;

namespace Pacman.Entities;


public class Coin : Entity
{
    public Coin() : base("pacman")
    {
        sprite.TextureRect = new IntRect(2 * 18, 2 * 18, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if (other is Pacman)
        {
            scene.Events.PublishGainScore(100);
            Dead = true;
        }
    }
}
