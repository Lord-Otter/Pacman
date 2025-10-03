using SFML.Graphics;
using SFML.Window;
using static SFML.Window.Keyboard.Key;

namespace Pacman.Entities;


public class Pacman : Actor
{
    public override void Create(Scene scene)
    {
        moving = false;
        speed = 100f;
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        scene.Events.LoseHealth += OnLoseHealth;
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.Events.LoseHealth -= OnLoseHealth;
    }

    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    protected override int PickDirection(Scene scene)
    {
        int dir = direction;
        if (Keyboard.IsKeyPressed(Right)) { dir = 0; moving = true; }
        else if (Keyboard.IsKeyPressed(Up)) { dir = 1; moving = true; }
        else if (Keyboard.IsKeyPressed(Left)) { dir = 2; moving = true; }
        else if (Keyboard.IsKeyPressed(Down)) { dir = 3; moving = true; }

        if (IsFree(scene, dir))
        {
            sprite.TextureRect = new IntRect(0, 18 * dir, 18, 18);
            return dir;
        }

        if (!IsFree(scene, direction)) moving = false;
        return direction;
    }
}
