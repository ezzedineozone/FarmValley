using System.Collections.Generic;
using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] GameEvents gameEvents;
    [SerializeField] GameObject playerPrefab;

    Dictionary<Item, int> inventory = new Dictionary<Item, int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameEvents.onItemPickup.AddListener(AddToInventory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AddToInventory(Item item)
    {
        Debug.Log("Item added to inventory: " + item.itemName);
        int amount = 1;
        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory.Add(item, amount);
        }
        gameEvents.onInventoryChange.Invoke(item, inventory.GetValueOrDefault(item, 0));
    }
}
