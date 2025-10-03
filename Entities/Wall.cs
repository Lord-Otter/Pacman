using SFML.Graphics;

namespace Pacman.Entities;


public sealed class Wall() : Entity("pacman")
{
    public override bool Solid => true;
    public override void Create(Scene scene)
    {
        base.Create(scene);
        sprite.TextureRect = new IntRect(3 * 18, 3 * 18, 18, 18);
    }

    public override void Update(Scene scene, float deltaTime) { }
}
