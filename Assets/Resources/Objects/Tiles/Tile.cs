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
    [SerializeField]
    private Textures texture;
    private Sprite textureObject;
    public bool is_animated;


    public bool IsPlayerInRange()
    {
        Transform ts = Character.instance.transform;
        float distance = Vector3.Distance(ts.position, this.transform.position);
        return distance < 1.5f;
    }

    public bool IsPlayerLookingAt()
    {
        var movement_mode = Character.instance.anim.GetFloat("movement_mode");
        var left_right_dir = Character.instance.anim.GetFloat("left_right_dir");
        var up_down_dir = Character.instance.anim.GetFloat("up_down_dir");
        var x = this.transform.position.x - Character.instance.transform.position.x;
        var y = this.transform.position.y - Character.instance.transform.position.y;
        if (movement_mode == 0)
        {
            if (left_right_dir > 0 && x > 0)
            {
                return true;
            }
            if (left_right_dir < 0 && x < 0)
            {
                return true;
            }
        }
        else
        {
            if (up_down_dir > 0 && y > 0)
            {
                return true;
            }
            if (up_down_dir < 0 && y < 0)
            {
                return true;
            }
        }
        return false;
    }

    public void SubscribeToPlayerInteraction()
    {
        Character.playerInteracted += OnPlayerInteraction;
    }
    public void OnPlayerInteraction()
    {
        StartCoroutine(OnPlayerInteractionCorutine());
    }
    public IEnumerator OnPlayerInteractionCorutine()
    {
        if (IsPlayerInRange() && IsPlayerLookingAt())
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
