using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PrestigeButton : MonoBehaviour,IAutoPressable
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
    private BuildingInstance _building;
    private bool _success;
    private int _counter;
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

        _counter = _event.MinValue;

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }



    private IEnumerator RandomTrigger()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_event.MinTime, _event.MaxTime) * _building.PeriodMultiplier());
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        _qteActive = true;
        _ui.SetActive(true);
        _success = false;

        float time = _event.Time;
        while (time > 0f && _qteActive)
        {
            time -= Time.deltaTime;

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_uiData.ScreenOffset;

            _labelText.text = _uiData.Prompt;
            _timeText.text = $"({Mathf.CeilToInt(time)}s)";
            yield return null;
        }


        _timeText.text = _success ? "Success" : "Failed";
        _image.color = _success ? Color.green : Color.red;
        if (!_success) OnFailed();
        yield return new WaitForSeconds(_uiData.UIShowTime);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (_qteActive)
        {
            ResourceType canister = _event.LookAtResource;
            _success = true;
            

            bool bonus = _counter >= 3;            
            foreach (var r in _event.Reward)
            {
                if (r.Type == canister) _building.PayOut(r.Type, r.Amount * _counter);
                else if(bonus) _building.PayOut(r.Type, r.Amount);
            }
            
            _building.CountProduction();
            _counter++;
            _qteActive = false;
        }
    }

    private void OnFailed()
    {
        if(_counter >0) _counter--;
        else if(_event.DestroyOnFail) _building.TryDestroy(this.gameObject);

    }

    private void Update()
    {
        Debug.Log(_counter);
    }
}
