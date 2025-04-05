using System;
using UnityEngine;

public interface IInteractibleObject
{
    void SubscribeToPlayerInteraction();
    void OnPlayerInteraction(Vector2 lookDir);
}
