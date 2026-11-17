using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Resource _resources;
    [SerializeField] private ResourceAmount[] _failLoss;
    [SerializeField, Range(0f, 1f)] private float _chance = 0.1f;

    private Generators_SO _data;
    private float _timer;

    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        _data = GetComponent<BuildingInstance>().Data as Generators_SO;
        if (_data == null)
        {
            Debug.LogWarning($"{name}:failed to get component", this);
            enabled = false;
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        while (_timer >= _data.GeneratePeriod)
        {
            _timer -= _data.GeneratePeriod;
            if(Random.value < _chance)
            {
                foreach (var p in _data.Production)
                    _resources.Add(p.Type, p.Amount);
            }
            else
            {
                _resources.SpendUpTo(_failLoss);
            }
        }
    }
}
