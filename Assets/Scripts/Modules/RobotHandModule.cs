using UnityEngine;

public class RobotHandModule : MonoBehaviour
{
    private BuildingInstance _building;
    private float _delay;
    private float _timer;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;
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
        _delay = _event.Time;
    }

    void Update()
    {
        if(_building.Grid == null) return;

        _timer += Time.deltaTime;
        if(_timer < _delay) return;

        if(TryPress(out string side))
        {
            _timer = 0f;
            Debug.Log($"pressed the {side}", this);
        }
        else
        {
            _timer = _delay;
        }

    }

    private bool TryPress(out string side)
    {
        side = null;
        foreach(var n in _building.Grid.Neighbours(_building.Cell))
        {
            if(!n.TryGetComponent(out IAutoPressable p) || !p.EventActive) continue;

            side = SideOf(n);
            p.OnPress();
            return true;
        }
        return false;
    }

    private string SideOf(BuildingInstance neighbour)
    {
        Vector3Int d = neighbour.Cell - _building.Cell;
        if (d.x > 0) return "right";
        if (d.x < 0) return "left";
        if (d.z > 0) return "up";
        return "down";
    }


}
