using System.Collections.Generic;
using UnityEngine;

public class TextureProvider
{
    private static Dictionary<Tile.Textures, string[]> textureMappings = new Dictionary<Tile.Textures, string[]>
    {
        { Tile.Textures.Grass, new string[] { "Grass_56"} },
        { Tile.Textures.TilledDirt, new string[] { "Tilled_Dirt_v2_51" } },
        { Tile.Textures.WoodenRoof, new string[] { "Wooden_House_Roof_Tilset_0" } },
        { Tile.Textures.Water, new string[] { "Water_0" } },
        {Tile.Textures.PlantedTilledDirt, new string[] {"Planted_Tilled_Dirt_Wide_v2_11"}}
    };
    private static Dictionary<Item.ItemTexture, string[]> itemTextureMappings = new Dictionary<Item.ItemTexture, string[]>
    {
        { Item.ItemTexture.Seeds, new string[] { "wheat_seeds_0" } }
        , { Item.ItemTexture.Wood, new string[] { "wooden_table" } },
        {Item.ItemTexture.Bed, new string[] {"bed_0"}},
        {Item.ItemTexture.Plant0, new string[] {"wheat_stages_2"}},
        {Item.ItemTexture.Plant1, new string[] {"wheat_stages_5"}},
        {Item.ItemTexture.Plant2, new string[] {"wheat_stages_7"}},
        {Item.ItemTexture.Wheat, new string[] {"wheat_0"}},

    };
    private static Dictionary<NPC.Texture, string[]> npcTextureMappings = new Dictionary<NPC.Texture, string[]>
    {
        { NPC.Texture.Horse, new string[] { "Horse_idle_side" } }
    };
    private static Dictionary<string, Sprite> loadedSprites;
    public static void LoadSprites()
    {
        if (loadedSprites == null)
        {
            loadedSprites = new Dictionary<string, Sprite>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/Tilesets");
            Sprite[] itemSprites = Resources.LoadAll<Sprite>("Textures/Items");
            Sprite[] npcSprites = Resources.LoadAll<Sprite>("Textures/NPCs/Horse");

            foreach (var sprite in sprites)
            {
                loadedSprites[sprite.name] = sprite;
            }
            foreach (var sprite in itemSprites)
            {
                loadedSprites[sprite.name] = sprite;
            }
            foreach (var sprite in npcSprites)
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
    public static Sprite GetItemSprite(Item.ItemTexture texture)
    {
        LoadSprites();

        if (itemTextureMappings.TryGetValue(texture, out string[] textureNames))
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
    public static Sprite GetNPCSprite(NPC.Texture texture)
    {
        LoadSprites();

        if (npcTextureMappings.TryGetValue(texture, out string[] textureNames))
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

