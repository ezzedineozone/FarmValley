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
        Water
    }
    public float z_index = -1.0f;
    [SerializeField]
    private Textures texture;
    private Sprite textureObject;
    public bool is_animated;
    [SerializeField] GameEvents gameEvents; 

    public bool IsPlayerInRange()
    {
        Transform ts = Character.instance.transform;
        float distance = Vector3.Distance(ts.position, this.transform.position);
        return distance < 1.5f;
    }

    public bool IsPlayerLookingAt(Vector2 lookDir)
    {
        Vector2 origin = (Vector2)Character.instance.transform.position + lookDir.normalized * 0.08f;
        RaycastHit2D hit = Physics2D.Raycast(origin, lookDir, 0.08f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Player is looking at the tile");
                return true;
            }
        }
        return false;
    }

    public void SubscribeToPlayerInteraction()
    {
        gameEvents.onPlayerInteract.AddListener(OnPlayerInteraction);
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
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeTexture(texture);
        SubscribeToPlayerInteraction();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z_index);
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

}
