using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private GameObject hotBarSlotPrefab;
    [SerializeField] private int numberOfSlots = 10;
    [SerializeField] private float slotSpacing = 5f;
    [SerializeField] GameEvents gameEvents;
    List<GameObject> hotBarSlots = new List<GameObject>();

    int filled_slots = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createHotbarSlots(numberOfSlots);
        gameEvents.onInventoryChange.AddListener(OnInventoryChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void createHotbarSlots(int amount)
    {
        if (hotBarSlotPrefab == null)
        {
            Debug.LogError("HotBarSlotPrefab is not assigned in the inspector.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject hotBarSlot = Instantiate(hotBarSlotPrefab, transform);
            RectTransform rectTransform = hotBarSlot.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.pivot = new Vector2(0, 0.5f);

            if (i > 0)
            {
                RectTransform previousSlot = hotBarSlots[i - 1].GetComponent<RectTransform>();
                float newX = previousSlot.anchoredPosition.x + previousSlot.rect.width + slotSpacing;
                rectTransform.anchoredPosition = new Vector2(newX, 0);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(0, 0);
            }

            hotBarSlots.Add(hotBarSlot);
        }
    }
    void OnInventoryChange(Item item, int amount)
    {
        // Find the first empty slot in the hotbar or the first slot with the same item
        if(filled_slots >= hotBarSlots.Count)
        {
            Debug.Log("Hotbar is full. Cannot add more items.");
            return;
        }
        for (int i = 0; i < hotBarSlots.Count; i++)
        {
            HotbarSlot slot = hotBarSlots[i].GetComponent<HotbarSlot>();
            if (slot.IsEmpty())
            {
                slot.SetItem(item, amount);
                slot.ChangeSlotDisplay();
                filled_slots++;
                break;
            }
        }
    }

}
