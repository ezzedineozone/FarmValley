using UnityEngine;

public interface IInteractibleWorldObject : IInteractibleObject
{
    bool IsPlayerInRange();
    bool IsPlayerLookingAt();
}
