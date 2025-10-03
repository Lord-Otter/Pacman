using Pacman.Managers;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Pacman;


class Program
{
    static void Main(string[] args)
    {
        using var window = new RenderWindow(new VideoMode(828, 900), "Pacman");
        window.Closed += (o, e) => window.Close();
        window.SetView(new View(new FloatRect(18, 0, 18 * 23, 18 * 25)));

        //Initialize
        Clock clock = new Clock();
        Scene scene = new Scene();
        scene.Loader.Load("maze");

        while (window.IsOpen)
        {
            window.DispatchEvents();

            float deltaTime = clock.Restart().AsSeconds();
            deltaTime = MathF.Min(deltaTime, 0.01f);
            //Updates
            scene.UpdateAll(deltaTime);

            window.Clear(new Color(223, 246, 245));
            //Drawing
            scene.RenderAll(window);
            window.Display();
        }
    }
}
