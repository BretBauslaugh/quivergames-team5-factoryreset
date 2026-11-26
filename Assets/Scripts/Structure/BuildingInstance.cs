using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  class BuildingInstance : MonoBehaviour
{
    [SerializeField] private Structure _data;
    [SerializeField] private Resource _resources;
    [SerializeField] private ResourceType _coinResource;

    public Structure Data => _data;
    public BuildingGrid Grid => _grid;
    public Vector3Int Cell { get; private set; }
    private Outline _outline;
    private BuildingGrid _grid;
    bool hovered, selected;

    private int _produceCount;
    private static FloatingText _popupPrefab;

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
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineWidth = 4f;
        _outline.enabled = false;
    }

    public void SetHovered(bool v)
    {
        hovered = v;
        Refresh();
        if (v) 
        { 
            DisplayDescription();
        }
        else
        {
            HideDescription();
        }
    }

    public void SetSelected(bool v)
    {
        selected = v;
        Refresh();
    }

    void Refresh()
    {
        if (_outline == null) return;
        _outline.enabled = selected || hovered;
        _outline.OutlineColor = selected ? Color.green : Color.yellow;
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
        if (type == null) return;
        int paid = amount;
        System.Collections.Generic.List<IPayoutListener> boosted = null;

        if (type.Enhanceable && _grid != null)
        {
            float flat = 0f, mult = 1f;
            foreach (var n in _grid.Neighbours(Cell))
            {
                if (n.TryGetComponent(out IFlatEnhancer f)) flat += f.FlatBonus;
                if (n.TryGetComponent(out IEnhancer e))
                {
                    mult *= e.Multiplier;
                    if (n.TryGetComponent(out IPayoutListener l))
                        (boosted ??= new()).Add(l);
                }
            }
            paid = Mathf.RoundToInt((amount + flat) * mult);
        }

        _resources.Add(type, paid);
        ShowPopup(type, paid);

        if (boosted != null)
        {
            foreach (var l in boosted) l.OnBoostedPayout();
        }
    }

    public void PayOut(ResourceAmount[] rewards)
    {
        foreach (var r in rewards) PayOut(r.Type, r.Amount);
    }

    public void PayOut(ResourceAmount[] rewards, int times)
    {
        foreach (var r in rewards) PayOut(r.Type, r.Amount * times);
    }

    private void ShowPopup(ResourceType type, int paid)
    {
        if (paid == 0 || type == null) return;
        if (_popupPrefab == null) _popupPrefab = Resources.Load<FloatingText>("FloatingText");
        if (_popupPrefab == null) return;

        Vector3 pos = transform.position + Vector3.up * 1.5f + Vector3.right * Random.Range(-0.3f, 0.3f);
        var popup = Instantiate(_popupPrefab, pos, Quaternion.identity);
        popup.Show($"{(paid > 0 ? "+" : "")}{paid} {type.DisplayName}", type.PopupColor);
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

    public void DisplayDescription(){
        GameObject[] textBoxes = GameObject.FindGameObjectsWithTag("ModuleInfoUI");
        if(textBoxes[0]!=null){
            textBoxes[0].GetComponent<Hovertext>().SetDescriptionText(_data.Description);
            textBoxes[0].GetComponent<Hovertext>().SetNameText(_data.DisplayName);
        }
    }

    public void HideDescription(){
        GameObject[] textBoxes = GameObject.FindGameObjectsWithTag("ModuleInfoUI");
        if(textBoxes[0]!=null){
            textBoxes[0].GetComponent<Hovertext>().SetDescriptionText("");
            textBoxes[0].GetComponent<Hovertext>().SetNameText("");
        }
    }

    public int GetPrice(){
        foreach (var cost in _data.Cost){
                return cost.Amount;
        }
        return 0;
    }

    public void ReportSuccess()
    {
        if (_grid == null) return;
        foreach (var n in new System.Collections.Generic.List<BuildingInstance>(_grid.Neighbours(Cell)))
            if (n.TryGetComponent(out IOutcomeListener l)) l.OnNeighborSuccess();
    }

    public void ReportFailure()
    {
        if (_grid == null) return;
        foreach (var n in new System.Collections.Generic.List<BuildingInstance>(_grid.Neighbours(Cell)))
            if (n.TryGetComponent(out IOutcomeListener l)) l.OnNeighborFailure();
    }

}
