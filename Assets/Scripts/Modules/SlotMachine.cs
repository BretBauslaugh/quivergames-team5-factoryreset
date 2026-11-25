using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Resource _resources;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;

    private BuildingInstance _building;
    private Generators_SO _data;
    private float _timer;

    private void Start()
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

        _data = _building.Data as Generators_SO;
        if (_data == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        float period = _data.GeneratePeriod * _building.PeriodMultiplier();

        _timer += Time.deltaTime;
        while (_timer >= period)
        {
            _timer -= period;
            if(Random.value < _event.Chance)
            {
                _building.PayOut(_data.Production);
                _building.CountProduction();
            }
            else
            {
                _resources.SpendUpTo(_event.FailLost);
            }
        }
    }
}
