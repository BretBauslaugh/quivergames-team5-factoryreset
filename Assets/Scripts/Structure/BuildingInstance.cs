using UnityEngine;

public abstract class BuildingInstance : MonoBehaviour
{
    protected abstract Structure StrucData { get; }

    protected virtual string ExtraInfo => "";
    protected virtual void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Preview")) return;
        PrintCurrentInfo();
    }

    void Update()
    {
        
    }

    protected virtual void PrintCurrentInfo()
    {
        Debug.Log($"Id = {StrucData.Id}, " + $"Name = {StrucData.DisplayName}, " +  
            $"{ExtraInfo}", this);
    }
}
