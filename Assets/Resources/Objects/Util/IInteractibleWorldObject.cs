using UnityEngine;

public interface IInteractibleWorldObject : IInteractibleObject
{
    bool IsPlayerInRange();
    bool IsPlayerLookingAt(Vector2 lookDir);
}
