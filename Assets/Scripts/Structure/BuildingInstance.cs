using UnityEngine;

public  class BuildingInstance : MonoBehaviour
{
    [SerializeField] private Structure _data;
    [SerializeField] private Resource _resources;

    public Structure Data => _data;

    protected virtual string ExtraInfo => "";
    protected virtual void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Preview")) return;
        //PrintCurrentInfo();
    }

    void Update()
    {
        
    }

    //protected virtual void PrintCurrentInfo()
    //{
    //    Debug.Log($"Id = {StrucData.Id}, " + $"Name = {StrucData.DisplayName}, " +  
    //        $"{ExtraInfo}", this);
    //}
}
