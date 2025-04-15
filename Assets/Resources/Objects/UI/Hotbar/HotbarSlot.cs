using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class HotbarSlot : MonoBehaviour
{
    struct Slot
    {
        public Item item;
        public int amount;
    }
    Slot slot;
    Image childImage;

    void Start()
    {
    }

    void Awake()
    {
        slot = new Slot();
        slot.item = null;
        slot.amount = 0;

        // Create a UI Image as a child of the slot
        GameObject childObject = new GameObject("ItemIcon");
        childObject.transform.SetParent(transform);
        childObject.transform.localPosition = Vector3.zero;
        childObject.transform.localScale = Vector3.one;

        // Add an Image component to the child object
        childImage = childObject.AddComponent<Image>();
        childImage.raycastTarget = false; // Disable raycast blocking for the image

        // Set the image to be fully transparent initially
        childImage.color = new Color(1f, 1f, 1f, 0f); // RGB = white, Alpha = 0 (fully transparent)
        childImage.rectTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
        childImage.sprite = null;
    }

    void Update()
    {
    }

    public bool IsEmpty()
    {
        if (slot.amount == 0)
        {
            return true;
        }
        return false;
    }

    public bool SetItem(Item item, int amount)
    {
        if (IsEmpty())
        {
            slot.item = item;
            slot.amount = amount;
            return true;
        }
        return false;
    }

    public int GetItemAmount()
    {
        return slot.amount;
    }

    public void ChangeSlotDisplay()
    {
        if (childImage == null)
        {
            Debug.LogError("Child image is not initialized.");
            return;
        }
        if (slot.item == null)
        {
            childImage.sprite = null;
            return;
        }
        Sprite texture = slot.item.getTexture();
        if (texture != null)
        {
            childImage.sprite = texture;
            childImage.color = new Color(1f, 1f, 1f, 1f);
            Debug.Log("Setting texture: " + texture.name);
        }
        else
        {
            Debug.LogError("Texture not found for item: " + slot.item.itemName);
        }
    }
}