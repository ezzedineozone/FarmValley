using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum Texture
    {
        Horse
        // Add other texture types as needed
    }

    public Texture texture { get; set; }
    public Sprite sprite { get; set; }

    protected Vector2 roaming_bounds = new Vector2(5, 5);

    protected bool isRoaming = false;

    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>()?.sprite;
    }

    public virtual void StartRoaming()
    {
        isRoaming = true;
    }

    public virtual void StopRoaming()
    {
        isRoaming = false;
        Debug.Log($"{name} stopped roaming.");
    }
    public virtual void UpdateTexture(NPC.Texture newTexture)
    {
        texture = newTexture;
        sprite = TextureProvider.GetNPCSprite(texture);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
