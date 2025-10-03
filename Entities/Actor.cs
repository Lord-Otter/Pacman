using SFML.Graphics;
using SFML.System;

namespace Pacman.Entities;


public class Actor : Entity
{
    private bool wasAligned;
    protected float speed;
    protected int direction;
    protected bool moving;
    protected Vector2f originalPosition;
    protected float originalSpeed;

    protected Vector2i textureOffset = new Vector2i(0, 0);
    private float animationTimer = 0f;
    protected int frame;

    public float GracePeriodTimer { get; private set; } = 0f;

    protected Actor() : base("pacman")
    {

    }

    public override FloatRect Bounds
    {
        get
        {
            FloatRect bounds = base.Bounds;
            bounds.Top += 3;
            bounds.Left += 3;
            bounds.Width -= 6;
            bounds.Height -= 6;
            return bounds;
        }
    }

    public override void Update(Scene scene, float deltaTime)
    {
        animationTimer += deltaTime;
        frame = (int)animationTimer % 2;

        GracePeriodTimer = MathF.Max(GracePeriodTimer - deltaTime, 0f);

        base.Update(scene, deltaTime);
        if (IsAligned)
        {
            if (!wasAligned)
            {
                direction = PickDirection(scene);
            }

            if (moving)
            {
                wasAligned = true;
            }
        }
        else
        {
            wasAligned = false;
        }

        if (!moving || GracePeriodTimer > 0f) return;

        Position += ToVector(direction) * (speed * deltaTime);
        Position = MathF.Floor(Position.X) switch
        {
            < 0 => new Vector2f(432, Position.Y),
            > 432 => new Vector2f(0, Position.Y),
            _ => Position
        };
    }

    protected bool IsAligned =>
        (int)MathF.Floor(Position.X) % 18 == 0 &&
        (int)MathF.Floor(Position.Y) % 18 == 0;

    protected void Reset()
    {
        wasAligned = false;
        Position = originalPosition;
        speed = originalSpeed;
        GracePeriodTimer = 1f;
    }

    public override void Create(Scene scene)
    {
        base.Create(scene);
        originalPosition = Position;
        originalSpeed = speed;
        wasAligned = false;
    }

    protected bool IsFree(Scene scene, int dir)
    {
        Vector2f at = Position + new Vector2f(9, 9);
        at += 18 * ToVector(dir);
        FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);
        return !scene.FindIntersects(rect).Any(e => e.Solid);
    }

    protected static Vector2f ToVector(int dir)
    {
        return dir switch
        {
            0 => new Vector2f(1, 0),  //right
            1 => new Vector2f(0, -1), //up
            2 => new Vector2f(-1, 0), //left
            3 => new Vector2f(0, 1),  //down
            _ => new Vector2f(0, 0)
        };
    }

    protected virtual int PickDirection(Scene scene)
    {
        return 0;
    }

    public override void Render(RenderTarget target)
    {
        sprite.TextureRect = new IntRect(textureOffset.X + 18 * frame, textureOffset.Y + sprite.TextureRect.Top, 18, 18);
        base.Render(target);
    }
}
