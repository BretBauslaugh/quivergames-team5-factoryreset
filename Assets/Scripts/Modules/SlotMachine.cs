using UnityEngine;

public class SlotMachine : TimeModule
{
    [SerializeField] private Resource _resources;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;


    protected override void Start()
    {
        base.Start();
        if (!enabled) return;
        _event = Building.Data.Event;
        _uiData = Building.Data.UI;
        _sfx = Building.Data.Sfx;
    }

    protected override void OnTick()
    {
        if (Random.value < _event.Chance)
        {
            Building.PayOut(Data.Production);
            Building.CountProduction();
        }
        else
        {
            _resources.SpendUpTo(_event.FailLost);
        }
    }
}
