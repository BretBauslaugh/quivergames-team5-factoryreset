using UnityEngine;

public class CounterModule : MonoBehaviour
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private int _amountPerCount = 10;
    [SerializeField] private float _payPeriod = 10f;

    private BuildingInstance _building;
    private float _timer;
    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }
        _building = GetComponent<BuildingInstance>();
    }

    void Update()
    {
        if (_building.Grid == null) return;

        float period = _payPeriod * _building.PeriodMultiplier();
        _timer += Time.deltaTime;
        while (_timer >= period)
        {
            _timer -= period;
            int total = CollectSideCounts();            
            if (total > 0) _building.PayOut(_type, _amountPerCount * total);
        }
    }

    private int CollectSideCounts()
    {
        int total = 0;
        foreach (var n in _building.Grid.Neighbours(_building.Cell))
        {
            if(n.Cell.z != _building.Cell.z) continue;
            total += n.AddProduceCount();
        }
        return total;
    }
}
