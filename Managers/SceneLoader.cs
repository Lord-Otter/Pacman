using System.Text;
using Pacman.Entities;
using SFML.System;

namespace Pacman.Managers;


public class SceneLoader
{
    private readonly Dictionary<char, Func<Entity>> loaders;
    private string currentScene = "", nextScene = "";

    public SceneLoader()
    {
        loaders = new Dictionary<char, Func<Entity>>{
                { '#', () => new Wall()},
                { 'g', () => new Ghost()},
                { 'p', () => new Entities.Pacman()},
                { '.', () => new Coin()},
                { 'c', () => new Candy()}
            };
    }

    private bool TryCreate(char symbol, out Entity created)
    {
        if (loaders.TryGetValue(symbol, out Func<Entity>? loader))
        {
            created = loader();
            return true;
        }

        created = null!;
        return false;
    }

    public void HandleSceneLoad(Scene scene)
    {
        if (nextScene == "") return;
        scene.Clear();

        string path = $"assets/{nextScene}.txt";

        int y = 0;
        foreach (string line in File.ReadLines(path, Encoding.UTF8))
        {
            for (int x = 0; x < line.Length; x++)
            {
                if (TryCreate(line[x], out Entity created))
                {
                    created.Position = new Vector2f(x * 18, y * 18);
                    scene.Spawn(created);
                }
            }

            y++;
        }
        if (!scene.FindByType<GUI>(out _)) // If there isn't already a GUI: add one
            scene.Spawn(new GUI()); // All scenes have a GUI

        currentScene = nextScene;
        nextScene = "";
    }

    public void Load(string scene) => nextScene = scene;
    public void Reload() => nextScene = currentScene;
}
