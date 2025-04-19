using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game/GameEvents")]
class GameEvents : ScriptableObject
{
    public UnityEvent<Vector2> onPlayerInteract = new UnityEvent<Vector2>();
    public UnityEvent<Vector2, Item> onPlayerInteractWithItem = new UnityEvent<Vector2, Item>();
    public UnityEvent<Item , int > onInventoryChange = new UnityEvent<Item , int >();
    public UnityEvent<Item> onItemPickup = new UnityEvent<Item>();
}
