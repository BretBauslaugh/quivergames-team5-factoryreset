using UnityEngine;

public  class BuildingInstance : MonoBehaviour
{
    [SerializeField] private Structure _data;
    [SerializeField] private Resource _resources;

    public Structure Data => _data;
    private Renderer rend;
    private Color baseColor;
    bool hovered, selected;

    protected virtual string ExtraInfo => "";
    protected virtual void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Preview")) return;
        //PrintCurrentInfo();
    }

    void Update()
    {
        
    }

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        baseColor = rend.material.color;
    }

    public void SetHovered(bool v)
    {
        hovered = v;
        Refresh();
    }

    public void SetSelected(bool v)
    {
        selected = v;
        Refresh();
    }

    void Refresh()
    {
        rend.material.color = selected ? Color.green : hovered ? Color.yellow : baseColor;
    }

    //protected virtual void PrintCurrentInfo()
    //{
    //    Debug.Log($"Id = {StrucData.Id}, " + $"Name = {StrucData.DisplayName}, " +  
    //        $"{ExtraInfo}", this);
    //}
}
