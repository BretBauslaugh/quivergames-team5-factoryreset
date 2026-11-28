using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PiggyBank : TimeModule
{
    [Header("UI")]
    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _image;

    public bool EventActive => _qteActive;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;

    private bool _qteActive = false;
    private Camera _cam;
    private bool _success;    
    private int _counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _event = Building.Data.Event;
        _uiData = Building.Data.UI;
        _sfx = Building.Data.Sfx;
        

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }

    private IEnumerator RandomTrigger()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_event.MinTime, _event.MaxTime) * Building.PeriodMultiplier());
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        _pressed = 0;
        _qteActive = true;
        _ui.SetActive(true);

        float time = _event.Time;
        while (time > 0f && _pressed < _event.PressNeeded)
        {
            time -= Time.deltaTime;

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_uiData.ScreenOffset;

            _labelText.text = $"PRESS {_pressed}/{_event.PressNeeded}";
            _timeText.text = $"({Mathf.CeilToInt(time)}s)";
            yield return null;

        }
        _counter += Mathf.Min(_pressed, _event.PressNeeded);
        _qteActive = false;
        _labelText.text = $"PRESS {_pressed}/{_event.PressNeeded}";
        yield return new WaitForSeconds(_uiData.UIShowTime);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (Time.timeScale == 0f) return;
        if (_qteActive)
        {
            _counter++;
        }
    }

    protected override void OnTick()
    {
        foreach (var r in Data.Production)
        {
            Building.PayOut(r.Type, r.Amount * _counter);
        }
        Building.CountProduction();
        if (_counter > 0) _counter--;

    }

    private void OnFailed()
    {


    }
}
