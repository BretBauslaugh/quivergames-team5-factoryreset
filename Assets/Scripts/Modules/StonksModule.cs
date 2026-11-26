using UnityEngine;

public interface IPayoutListener { void OnBoostedPayout(); }

public class StonksModule : MonoBehaviour, IEnhancer, IPayoutListener, IOutcomeListener
{
    [SerializeField] private float _growth = 0.10f;
    [SerializeField] private GameObject _wreckagePrefab;

    private float _current;
    private BuildingInstance _building;
    private bool _exploded;

    void Awake()
    {
        _building = GetComponent<BuildingInstance>();
        _current = 1f + _growth;
    }

    public float Multiplier => _current;
    public void OnBoostedPayout() => _current *= 1f + _growth;

    public void OnNeighborSuccess() { }

    public void OnNeighborFailure()
    {
        if (_exploded) return;
        _exploded = true;
        if (_wreckagePrefab != null && _building.Grid != null)
        {
            var w = Instantiate(_wreckagePrefab, transform.position, transform.rotation);
            if (w.TryGetComponent(out BuildingInstance wb)) wb.Init(_building.Grid, _building.Cell);
        }
        Destroy(gameObject);
    }
}