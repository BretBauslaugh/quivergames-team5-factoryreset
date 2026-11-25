using UnityEngine;

public class CounterModule : MonoBehaviour
{


    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;

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
        _event = _building.Data.Event;
        _uiData = _building.Data.UI;
        _sfx = _building.Data.Sfx;
    }

    void Update()
    {
        if (_building.Grid == null) return;

        float period = _event.Time * _building.PeriodMultiplier();
        _timer += Time.deltaTime;
        while (_timer >= period)
        {
            _timer -= period;
            int total = CollectSideCounts();            
            if (total > 0) _building.PayOut(_event.Reward, total);
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
