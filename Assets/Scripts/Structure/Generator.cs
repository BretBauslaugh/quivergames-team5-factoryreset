using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(BuildingInstance))]
public class Generator : MonoBehaviour
{
    [SerializeField] private Resource _resources;
    [SerializeField] private TMP_Text _storedDisplay;
    [SerializeField] private GameObject _ui;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, -80);

    private Generators_SO _data;
    private float _timer;
    private BuildingInstance _building;
    private readonly Dictionary<ResourceType, int> _stored = new();
    private Camera _cam;

    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            _ui.SetActive(false);
            return;
        }

        _building = GetComponent<BuildingInstance>();
        _data = _building.Data as Generators_SO;
        if (_data == null)
        {
            Debug.LogWarning($"{name}:failed to get component", this);
            enabled = false;
        }

        _cam = Camera.main;
        _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_screenOffset;
    }

    private void Update()
    {
        float period = _data.GeneratePeriod * _building.PeriodMultiplier();
        _timer += Time.deltaTime;
        while (_timer >= period)
        {
            _timer -= period;
            Store();
            
        }
    }

    private void Store()
    {
        foreach(var p in _data.Production)
        {
            int stored = GetStored(p.Type);
            int space = _data.MaxCapacity - stored;
            if (space <= 0) continue;
            _stored[p.Type] = stored + Mathf.Min(p.Amount, space);
            _building.CountProduction();
            RefreshDisplay();
        }
    }

    public int GetStored(ResourceType type) => _stored.TryGetValue(type, out var v) ? v :0;

    public void Collect()
    {
        foreach(var r in _stored)
        {
            _building.PayOut(r.Key, r.Value);
        }
        _stored.Clear();
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        var sb = new System.Text.StringBuilder();
        foreach(var r in _stored)
        {
            sb.AppendLine($"{r.Key.DisplayName} : {r.Value}");
        }
        _storedDisplay.text = sb.ToString();
    }


}
