using SFML.Graphics;

namespace Pacman.Managers;


public class AssetManager
{
    public static readonly string AssetPath = "assets";
    private readonly Dictionary<string, Texture> textures;
    private readonly Dictionary<string, Font> fonts;

    public AssetManager()
    {
        textures = new Dictionary<string, Texture>();
        fonts = new Dictionary<string, Font>();
    }

    public Texture LoadTexture(string name)
    {
        if (textures.TryGetValue(name, out Texture? cached))
        {
            return cached;
        }
        Texture texture = new Texture($"assets/{name}.png");
        textures.Add(name, texture);
        return texture;
    }

    public Font LoadFont(string name)
    {
        if (fonts.TryGetValue(name, out Font? cached))
        {
            return cached;
        }
        Font font = new Font($"assets/{name}.ttf");
        fonts.Add(name, font);
        return font;
    }
}
