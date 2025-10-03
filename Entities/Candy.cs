using Pacman.Managers;
using SFML.Graphics;

namespace Pacman.Entities;


public class Candy : Entity
{
    public Candy() : base("pacman")
    {
        sprite.TextureRect = new IntRect(3 * 18, 2 * 18, 18, 18);
    }

    protected override void CollideWith(Scene scene, Entity other)
    {
        if (other is Pacman)
        {
            scene.Events.PublishCandyEaten();
            Dead = true;
        }
    }
}
