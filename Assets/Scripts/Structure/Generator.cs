using UnityEngine;

[RequireComponent(typeof(BuildingInstance))]
public class Generator : MonoBehaviour
{
    [SerializeField] private Resource _resources;
    

    private Generators_SO _data;
    private float _timer;
    private BuildingInstance _building;

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
            _building.PayOut(_data.Production);
        }
    }
}
