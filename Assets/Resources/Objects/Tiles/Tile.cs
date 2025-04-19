using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Tile : MonoBehaviour, IInteractibleWorldObject
{
    public enum Textures
    {
        Grass,
        TilledDirt,
        WoodenRoof,
        Water,
        PlantedTilledDirt
    }
    public float z_index = -1.0f;
    [SerializeField]
    private Textures texture;
    private Sprite textureObject;
    public bool is_animated;
    bool is_planted;
    [SerializeField] GameEvents gameEvents; 
    [SerializeField] private GameObject storedItemPrefab;

    public bool IsPlayerInRange()
    {
        Transform ts = Character.instance.transform;
        float distance = Vector3.Distance(ts.position, this.transform.position);
    
        return distance < 1.5f;
    }

    public bool IsPlayerLookingAt(Vector2 lookDir)
    {
        Vector2 origin = (Vector2)Character.instance.transform.position + lookDir.normalized * 0.008f;
        int layerMask = LayerMask.GetMask("TileLayer");
        Debug.Log("Origin: " + origin);
        Debug.Log("Look Direction: " + lookDir);
        RaycastHit2D hit = Physics2D.Raycast(origin, lookDir, 0.08f, layerMask);
        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject == this.gameObject)
            {
                return true;
            }
            else
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name + " is not the tile.");
            }
        }
        return false;
    }

    public void SubscribeToPlayerInteraction()
    {
        gameEvents.onPlayerInteract.AddListener(OnPlayerInteraction);
        gameEvents.onPlayerInteractWithItem.AddListener(OnPlayerInteractionWithItem);
    }
    public void OnPlayerInteractionWithItem(Vector2 lookDir, Item item)
    {
        Debug.Log("OnPlayerInteractionWithItem: " + item);
        Debug.Log(item);
        if (item != null && item.itemID == 1 && IsPlayerInRange() && IsPlayerLookingAt(lookDir))
        {
            this.Plant(item);
        }
    }
    public void OnPlayerInteraction(Vector2 lookDir)
    {
        StartCoroutine(OnPlayerInteractionCorutine(lookDir));
    }
    public IEnumerator OnPlayerInteractionCorutine(Vector2 lookDir)
    {
        if (IsPlayerInRange() && IsPlayerLookingAt(lookDir))
        {
            yield return new WaitForSeconds(1f);
            ChangeTexture(Tile.Textures.TilledDirt);
            if(storedItemPrefab != null)
            {
                if(this.texture == Tile.Textures.Grass){
                    GameObject item = Instantiate(storedItemPrefab, this.transform.position, Quaternion.identity);
                    item.GetComponent<Item>().itemName = "Seeds";
                    item.GetComponent<Item>().itemID = 1;
                    item.GetComponent<Item>().itemDescription = "Seeds for planting.";
                    item.GetComponent<Item>().texture = Item.ItemTexture.Seeds;

                    StartCoroutine(DropItemCorutine(item, lookDir));


                }
                storedItemPrefab = null;
            }
            else
            {
                Debug.LogError("Stored Item Prefab is not assigned in the inspector.");
            }
        } else {

        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeTexture(texture);
        SubscribeToPlayerInteraction();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z_index);
        gameObject.layer = LayerMask.NameToLayer("TileLayer");
        Debug.Log(gameObject.layer);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeTexture(Tile.Textures texture)
    {
        textureObject = TextureProvider.GetSprite(texture);
        GetComponent<SpriteRenderer>().sprite = textureObject;
    }
IEnumerator DropItemCorutine(GameObject item, Vector2 dir)
{
    // Disable collider initially to prevent immediate interaction
    item.GetComponent<Collider2D>().enabled = false;
    
    // Get Rigidbody2D component for physics interactions
    Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
    
    // Set gravity and other physics properties
    rb.linearDamping = 2f;  // Apply drag for friction effect
    
    // Add an upward force and set linear velocity for a more dynamic motion
    rb.linearVelocity = Vector2.zero;  // Reset velocity before applying forces
    rb.AddForce(new Vector2(dir.x, 1f) * 5f, ForceMode2D.Impulse);  // Slight upward force
    rb.AddForce(dir * 2f, ForceMode2D.Impulse);  // Apply horizontal force
    
    // Allow item to move for a short period
    yield return new WaitForSeconds(1f);
    
    // Re-enable collider after the item settles
    item.GetComponent<Collider2D>().enabled = true;
}
    void Plant(Item item){
        ChangeTexture(Tile.Textures.PlantedTilledDirt);
        is_planted = true;
        GameObject.FindGameObjectWithTag("hotbar_main").GetComponent<Hotbar>().selectedSlot.clearSlot();
    }
}
