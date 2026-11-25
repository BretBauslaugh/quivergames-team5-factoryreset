using UnityEngine;
using UnityEngine.UIElements;

public class SavingAccountModule : TimeModule
{
    [SerializeField] private Resource _resource;

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
        ResourceType lookAt = _event.LookAtResource;
        int heldAmount = _resource.Get(lookAt);
        int counted = Mathf.Min((int)(heldAmount / _event.Divider), _event.MaxValue);

        foreach(var r in Data.Production)
        {
            int amount = r.Type == lookAt ? r.Amount * counted : r.Amount * heldAmount;
            if (amount > 0) Building.PayOut(r.Type, amount);
        }

        Building.CountProduction();

    }
}
