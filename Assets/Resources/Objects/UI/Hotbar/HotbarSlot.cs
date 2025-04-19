using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class HotbarSlot : MonoBehaviour
{
    public struct Slot
    {
        public Item item;
        public int amount;
    }
    public Slot slot;
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
            if (item == null)
            {
                Debug.LogError("Cannot set a null item.");
                return false;
            }

            // Duplicate the item
            Item duplicatedItem = Instantiate(item);
            duplicatedItem.name = item.name + "_Copy"; // Optional: Rename the duplicated item for clarity

            // Assign the duplicated item and amount to the slot
            slot.item = duplicatedItem;
            slot.amount = amount;
            //make slot.item invisible
            duplicatedItem.gameObject.SetActive(false);

            Debug.Log($"Item duplicated and set: {duplicatedItem.itemName}, Amount: {amount}");
            return true;
        }

        Debug.LogWarning("Slot is not empty. Cannot set item.");
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
    public void Select()
    {
        if (childImage == null)
        {
            Debug.LogError("Child image is not initialized.");
            return;
        }

        // Create a new temporary Image object
        GameObject tempObject = new GameObject("SelectedItemHighlight");
        tempObject.transform.SetParent(transform);
        tempObject.transform.localPosition = Vector3.zero;
        tempObject.transform.localScale = new Vector3(1f, 1f, 1f);

        Image tempImage = tempObject.AddComponent<Image>();
        tempImage.sprite = childImage.sprite; // Use the same sprite as the childImage
        tempImage.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent white
        tempImage.raycastTarget = false; // Disable raycast blocking for the temporary image
    }

    public void DeSelect()
    {
        if (childImage == null)
        {
            Debug.LogError("Child image is not initialized.");
            return;
        }

        // Reset the childImage to its original state
        if(slot.item != null)
        {
            childImage.color = new Color(1f, 1f, 1f, 0f); // Fully transparent
            childImage.rectTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }



        // Destroy any temporary highlight objects
        Transform highlight = transform.Find("SelectedItemHighlight");
        if (highlight != null)
        {
            Destroy(highlight.gameObject);
        }
        ChangeSlotDisplay();
    }
    public void clearSlot(){
        if (slot.item != null)
        {
            Destroy(slot.item.gameObject); // Destroy the item GameObject
            slot.item = null; // Set the item to null
            slot.amount = 0; // Reset the amount to 0
            childImage.sprite = null; // Clear the sprite
            ChangeSlotDisplay(); // Update the display
        }
    }
    public Item GetItem(){
        return slot.item;
    }
}