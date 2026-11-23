using UnityEngine;
using TMPro;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class UnstableModule : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _minTime = 10f;
    [SerializeField] private float _maxTime = 20f;
    [SerializeField] private float _timeLimit = 5f;
    [SerializeField] private int _buttonPress = 3;

    [Header("UI")]
    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, 200f);
    [SerializeField] private float _uiLastTime;
    
    [Header("FMOD")]
    [SerializeField] private EventReference _appearSFX;
    [SerializeField] private EventReference _completeSFX;
    [SerializeField] private EventReference _failSFX;
    [SerializeField] private EventReference _onPressSFX;

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
        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }

    private IEnumerator RandomTrigger()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minTime, _maxTime) * _building.PeriodMultiplier());
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        _pressed = 0;
        _qteActive = true;
        Playsfx(_appearSFX);
        _ui.SetActive(true);

        float time = _timeLimit;
        while(time > 0f && _pressed < _buttonPress)
        {
            time -= Time.deltaTime;

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) +  (Vector3)_screenOffset;

            _labelText.text = $"PRESS {_pressed}/{_buttonPress}";
            _timeText.text = $"({Mathf.CeilToInt(time)}s)";
            yield return null;
            
        }

        _qteActive = false;
        bool success = _pressed >= _buttonPress;
        _labelText.text = $"PRESS {_pressed}/{_buttonPress}";
        _timeText.text = success ? "Success" : "Failed";
        _image.color = success ? Color.green : Color.red;
        if (!success) OnFailed();
        if (success) Playsfx(_completeSFX);
        yield return new WaitForSeconds(_uiLastTime);
        _ui.SetActive(false); 
    }

    public void OnPress()
    {
        Playsfx(_onPressSFX);
        if (Time.timeScale == 0f) return;
        if (_qteActive) _pressed++;
    }

    private void OnFailed()
    {
        //Explosion
        Playsfx(_failSFX);
        Destroy(gameObject, 1f);
    }

}
