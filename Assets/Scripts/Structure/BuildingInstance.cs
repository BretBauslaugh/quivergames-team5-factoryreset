using UnityEngine;
using UnityEngine.EventSystems;

public  class BuildingInstance : MonoBehaviour
{
    [SerializeField] private Structure _data;
    [SerializeField] private Resource _resources;

    public Structure Data => _data;
    public BuildingGrid Grid => _grid;
    public Vector3Int Cell { get; private set; }
    private Renderer rend;
    private Color baseColor;
    private BuildingGrid _grid;
    bool hovered, selected;

    private int _produceCount;

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

    public void Init(BuildingGrid grid, Vector3Int cell)
    {
        _grid = grid;
        Cell = cell;
        grid.Register(this, Cell);
    }

    private void OnDestroy()
    {
        if(_grid != null)
        {
            _grid.Unregister(this);
        }
    }

    public float ProductionMultiplier()
    {
        float mult = 1f;
        if (_grid == null) return mult;
        foreach (var n in _grid.Neighbours(Cell))
        {
            if (n.TryGetComponent(out IEnhancer e)) mult *= e.Multiplier;
        }
        return mult;
    }

    public float PeriodMultiplier()
    {
        float mult = 1f;
        if (_grid == null) return mult;
        foreach (var n in _grid.Neighbours(Cell))
        {
            if(n.Cell.z != Cell.z) continue;
            if (n.TryGetComponent(out ITimeSkip t)) mult *= t.TimeMultiplier;
        }
        return mult;
    }

    public void PayOut(ResourceType type, int amount)
    {
        _resources.Add(type, Mathf.RoundToInt(amount * ProductionMultiplier()));
    }

    public void PayOut(ResourceAmount[] rewards)
    {
        foreach (var r in rewards) PayOut(r.Type, r.Amount);
    }

    public void CountProduction() => _produceCount++;

    //public void CountProduction()
    //{
    //    _produceCount++;
    //    Debug.Log($"Production Count: {_produceCount}");
    //}


    //public void OnClicked() => PayOut(_data.ClickReward);

    public int AddProduceCount()
    {
        int c = _produceCount;
        return c;
    }

    //protected virtual void PrintCurrentInfo()
    //{
    //    Debug.Log($"Id = {StrucData.Id}, " + $"Name = {StrucData.DisplayName}, " +  
    //        $"{ExtraInfo}", this);
    //}

    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("mouseOver");
        GameObject[] textBoxes = GameObject.FindGameObjectsWithTag("ModuleInfoUI");
        if(textBoxes[0]!=null){
            textBoxes[0].GetComponent<Hovertext>().SetDescriptionText(_data.Description);
            textBoxes[0].GetComponent<Hovertext>().SetNameText(_data.DisplayName);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        Debug.Log("mouseOut");
        GameObject[] textBoxes = GameObject.FindGameObjectsWithTag("ModuleInfoUI");
        if(textBoxes[0]!=null){
            textBoxes[0].GetComponent<Hovertext>().SetDescriptionText("");
            textBoxes[0].GetComponent<Hovertext>().SetNameText("");
        }
    }

}
