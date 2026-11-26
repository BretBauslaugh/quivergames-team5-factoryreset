using UnityEngine;

public interface IAutoPressable
{
    bool EventActive { get; }
    void OnPress();
}
