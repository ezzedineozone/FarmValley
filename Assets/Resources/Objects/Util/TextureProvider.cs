using System.Collections.Generic;
using UnityEngine;

public class TextureProvider
{
    private static Dictionary<Tile.Textures, string[]> textureMappings = new Dictionary<Tile.Textures, string[]>
    {
        { Tile.Textures.Grass, new string[] { "Grass_0"} },
        { Tile.Textures.TilledDirt, new string[] { "Tilled_Dirt_Wide_v2_0" } },
        { Tile.Textures.WoodenRoof, new string[] { "Wooden_House_Roof_Tilset_0" } },
        { Tile.Textures.Water, new string[] { "Water_0" } }
    };
    private static Dictionary<string, Sprite> loadedSprites;
    public static void LoadSprites()
    {
        if (loadedSprites == null)
        {
            loadedSprites = new Dictionary<string, Sprite>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/Tilesets");

            foreach (var sprite in sprites)
            {
                loadedSprites[sprite.name] = sprite;
            }
        }
    }

    public static Sprite GetSprite(Tile.Textures texture)
    {
        LoadSprites();

        if (textureMappings.TryGetValue(texture, out string[] textureNames))
        {
            int randomIndex = Random.Range(0, textureNames.Length);
            string name = textureNames[randomIndex];
            if (loadedSprites.TryGetValue(name, out Sprite sprite))
            {
                return sprite;
            }
        }

        return null;
    }
}
