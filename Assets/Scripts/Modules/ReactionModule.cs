using UnityEngine;
using TMPro;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class ReactionModule : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _image;
    
    [Header("FMOD")]
    [SerializeField] private EventReference _appearSFX;
    [SerializeField] private EventReference _completeSFX;
    [SerializeField] private EventReference _failSFX;
    [SerializeField] private EventReference _onPressSFX;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;

    private bool _qteActive = false;
    private Camera _cam;
    private BuildingInstance _building;
    private bool _success;
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

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }
    
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
        Playsfx(_appearSFX);
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
        if (_success) Playsfx(_completeSFX);
        yield return new WaitForSeconds(_uiData.UIShowTime);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (_qteActive)
        {
            _success = true;
            _building.PayOut(_event.Reward);
            _building.CountProduction();
            _qteActive = false;
            Playsfx(_onPressSFX);
        }
    }

    private void OnFailed()
    {
        Playsfx(_failSFX);        
    }
}
