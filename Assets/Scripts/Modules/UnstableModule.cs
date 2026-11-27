using UnityEngine;
using TMPro;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class UnstableModule : MonoBehaviour, IAutoPressable
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

    private int _pressed = 0;
    private bool _qteActive = false;
    private Camera _cam;
    private BuildingInstance _building;

    public void Playsfx(EventReference sfx)
    {
        if (sfx.IsNull)
        {
            Debug.LogError("ERROR: [FMOD] Event not found.");
            return;
        }
        EventInstance instance = RuntimeManager.CreateInstance(sfx);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position)); //added to prevent warnings
        RuntimeManager.PlayOneShot(sfx, transform.position);
    }
    
    private void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        _building = GetComponent<BuildingInstance>();
        _event = _building.Data.Event;
        _uiData = _building.Data.UI;
        _sfx = _building.Data.Sfx;

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }

    private IEnumerator RandomTrigger()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_event.MinTime, _event.MinTime) * _building.PeriodMultiplier());
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        _pressed = 0;
        _qteActive = true;
        Playsfx(_sfx.Appear);
        _ui.SetActive(true);

        float time = _event.Time;
        while(time > 0f && _pressed < _event.PressNeeded)
        {
            time -= Time.deltaTime;

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) +  (Vector3)_uiData.ScreenOffset;

            _labelText.text = $"PRESS {_pressed}/{_event.PressNeeded}";
            _timeText.text = $"({Mathf.CeilToInt(time)}s)";
            yield return null;
            
        }

        _qteActive = false;
        bool success = _pressed >= _event.PressNeeded;
        _labelText.text = $"PRESS {_pressed}/{_event.PressNeeded}";
        _timeText.text = success ? "Success" : "Failed";
        _image.color = success ? Color.green : Color.red;
        if (!success) OnFailed();
        if (success) Playsfx(_sfx.Complete);
        yield return new WaitForSeconds(_uiData.UIShowTime);
        _ui.SetActive(false); 
    }

    public void OnPress()
    {
        Playsfx(_sfx.Press);
        if (Time.timeScale == 0f) return;
        if (_qteActive) _pressed++;
    }

    private void OnFailed()
    {
        //Explosion
        Playsfx(_sfx.Fail);
        if(_event.DestroyOnFail) _building.TryDestroy(this.gameObject);

    }

}
