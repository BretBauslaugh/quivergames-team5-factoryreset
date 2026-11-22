using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    public ResourceAmount[] _startingAmount;
    [SerializeField] private GameEvent _onChanged;

    private readonly Dictionary<ResourceType, int> _amounts = new();

    private void OnEnable()
    {
        ResetToStart();
    }

    public int Get(ResourceType t) => _amounts.TryGetValue(t, out var v) ? v : 0;
    public void Add(ResourceType t, int amount)
    {
        _amounts[t] = Get(t) + amount;
        RaiseChanged();
    }

    public bool CanAfford(ResourceAmount[] cost)
    {
        foreach(var c  in cost)
        {
            if(Get(c.Type) < c.Amount) return false;
        }
        return true;
    }

    public bool TryGetMissing(ResourceAmount[] cost, out ResourceType missing)
    {
        foreach(var c in cost)
        {
            if(Get(c.Type) < c.Amount)
            {
                missing = c.Type;
                return true;
            }
        }
        missing = null;
        return false;
    }

    public void Spend(ResourceAmount[] cost)
    {
        foreach(var c in cost)
        {
            Add(c.Type, -c.Amount);
        }
    }

    public void SpendUpTo(ResourceAmount[] amounts)
    {
        foreach (var a in amounts)
        {
            int take = Mathf.Min(a.Amount, Get(a.Type));
            if (take > 0) Add(a.Type, -take);
        }
    }

    public void ResetToStart()
    {
        _amounts.Clear();
        foreach (var s in _startingAmount) Add(s.Type, s.Amount);
        RaiseChanged();
    }

    //public void LogAll()
    //{
    //    foreach (var kv in _amounts)
    //        Debug.Log($"{kv.Key.DisplayName}: {kv.Value}");
    //}

    private void RaiseChanged()
    {
        if (_onChanged != null) _onChanged.Raise();
    }


}
