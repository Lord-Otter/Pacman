using Pacman.Entities;
using SFML.Graphics;
using Pacman.Managers;

namespace Pacman;


public sealed class Scene
{
    private List<Entity> entities = new List<Entity>();
    public readonly SceneLoader Loader = new SceneLoader();
    public readonly AssetManager Assets = new AssetManager();
    public readonly EventManager Events = new EventManager();

    public void Spawn(Entity entity)
    {
        entities.Add(entity);
        entity.Create(this);
    }

    public void Clear()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            if (!entity.DontDestroyOnLoad)
            {
                entities.RemoveAt(i);
                entity.Destroy(this);
            }
        }
    }

    public void UpdateAll(float deltaTime)
    {
        Loader.HandleSceneLoad(this);

        // Update entities
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].Update(this, deltaTime);
        }

        Events.DispatchEvents(this);

        // Remove dead entities
        for (int i = 0; i < entities.Count;)
        {
            if (entities[i].Dead)
            {
                entities.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    public void RenderAll(RenderTarget target)
    {
        foreach (Entity entity in entities)
        {
            entity.Render(target);
        }
    }

    public bool FindByType<T>(out T found) where T : Entity
    {
        found = entities.OfType<T>().Where(e => !e.Dead).FirstOrDefault()!;
        return found != null;
    }

    public IEnumerable<Entity> FindIntersects(FloatRect bounds)
    {
        int lastEntity = entities.Count - 1;
        for (int i = lastEntity - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            if (entity.Dead)
            {
                continue;
            }
            if (entity.Bounds.Intersects(bounds))
            {
                yield return entity;
            }
        }
    }
}
