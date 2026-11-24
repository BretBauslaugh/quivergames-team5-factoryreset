using UnityEngine;
using TMPro;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class BasicModule : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private float _time;
    [SerializeField] private int _addMount;

    [Header ("UI")]
    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, 200f);
    [SerializeField] private float _uiLastTime;
    
    [Header("FMOD")]
    [SerializeField] private EventReference _appearSFX;
    [SerializeField] private EventReference _completeSFX;
    [SerializeField] private EventReference _failSFX;
    [SerializeField] private EventReference _onPressSFX;

    private bool _qteActive = false;
    private Camera _cam;
    private BuildingInstance _building;

    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        _building = GetComponent<BuildingInstance>();
        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(StartTimer());
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

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_time * _building.PeriodMultiplier());
            _qteActive = true;
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        
        _ui.SetActive(true);
        Playsfx(_appearSFX);

        
        while (_qteActive)
        {

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_screenOffset;
            _labelText.text = $"PRESS THE BUTTON";
            yield return null;

        }              
        Playsfx(_completeSFX);
        _image.color = Color.green;        
        yield return new WaitForSeconds(_uiLastTime);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        Playsfx(_onPressSFX);
        if (_qteActive) 
        {
            _building.PayOut(_type, _addMount);
            _building.CountProduction();
            _qteActive = false;
        }
    }

}
