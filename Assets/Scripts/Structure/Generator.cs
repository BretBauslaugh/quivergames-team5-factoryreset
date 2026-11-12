using UnityEngine;

[RequireComponent(typeof(BuildingInstance))]
public class Generator : MonoBehaviour
{
    [SerializeField] private Resource _resources;
    [SerializeField] private float _tickInterval = 1f;

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
        while (_timer >= _tickInterval)
        {
            _timer -= _tickInterval;
            foreach (var p in _data.Production)
                _resources.Add(p.Type, p.Amount);
        }
    }
}
