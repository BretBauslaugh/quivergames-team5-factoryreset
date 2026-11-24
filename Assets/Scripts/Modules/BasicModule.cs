using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class BasicModule : MonoBehaviour
{

    [Header ("UI")]
    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private Image _image;

    private EventSettings _event;
    private UISettings _uiData;
    private SfxSettings _sfx;

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
        _event = _building.Data.Event;
        _uiData = _building.Data.UI;
        _sfx = _building.Data.Sfx;

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_event.Time * _building.PeriodMultiplier());
            _qteActive = true;
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        
        _ui.SetActive(true);

        
        while (_qteActive)
        {

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_uiData.ScreenOffset;
            _labelText.text = $"PRESS THE BUTTON";
            yield return null;

        }              
        
        _image.color = Color.green;        
        yield return new WaitForSeconds(_uiData.UIShowTime);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (_qteActive) 
        {
            _building.PayOut(_event.Reward);
            _building.CountProduction();
            _qteActive = false;
        }
    }

}
