using System;
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Item : MonoBehaviour
{
    public string itemName;
    public int itemID;
    public string itemDescription;
    [SerializeField]
    public ItemTexture texture;
    [SerializeField]
    public bool isPickable = true;
    [SerializeField]
    private GameEvents gameEvents;

    private Sprite textureObject;

    public enum ItemTexture {
        Seeds,
        Wood,
        Bed,
        Plant0,
        Plant1,
        Plant2,
        Wheat
    }
    public virtual void Use()
    {
        Debug.Log("Using item: " + itemName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.texture == ItemTexture.Bed)
        {
            gameEvents.onPlayerSleep.Invoke();
        }
        if(!isPickable) return;
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Item picked up: " + itemName);
            Destroy(gameObject);
        }
    }
    void ChangeTexture(Item.ItemTexture texture)
    {
        textureObject = TextureProvider.GetItemSprite(texture);
        GetComponent<SpriteRenderer>().sprite = textureObject;
    }
    public Sprite getTexture()
    {
        textureObject = TextureProvider.GetItemSprite(texture);
        return textureObject;
    }
    public string GetItemDetails()
    {
        return $"{itemName} (ID: {itemID}) - {itemDescription}";
    }
    void Start()
    {
        ChangeTexture(texture);
    }
}