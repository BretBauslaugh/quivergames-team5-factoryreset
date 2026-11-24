using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Resource _resources;
    [SerializeField] private ResourceAmount[] _failLoss;
    [SerializeField, Range(0f, 1f)] private float _chance = 0.1f;

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
            if(Random.value < _chance)
            {
                _building.PayOut(_data.Production);
                _building.CountProduction();
            }
            else
            {
                _resources.SpendUpTo(_failLoss);
            }
        }
    }
}
