using UnityEngine;

[RequireComponent (typeof(BuildingInstance))]
public abstract class TimeModule : MonoBehaviour
{
    protected BuildingInstance Building;
    protected Generators_SO Data { get; private set; }

    private float _timer;
    protected virtual void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        Building = GetComponent<BuildingInstance>();
        Data = Building.Data as Generators_SO;
        if (Data == null)
        {
            Debug.LogWarning($"{name}: needs a Generators_SO", this);
            enabled = false;
        }
    }
    
    private void Update()
    {
        float period = Data.GeneratePeriod * Building.PeriodMultiplier();
        if(period <= 0 ) return;

        _timer += Time.deltaTime;
        while(_timer >= period)
        {
            _timer -= period;
            OnTick();
        }
    }

    protected abstract void OnTick();
}
