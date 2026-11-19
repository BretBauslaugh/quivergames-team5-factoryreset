using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ReactionModule : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private float _minTime = 10f;
    [SerializeField] private float _maxTime = 20f;
    [SerializeField] private float _timeLimit = 5f;
    [SerializeField] private int _addMount;

    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, 200f);

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
        _qteActive = true;
        _ui.SetActive(true);
        _success = false;

        float time = _timeLimit;
        while (time > 0f && _qteActive)
        {
            time -= Time.deltaTime;

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_screenOffset;

            _labelText.text = $"PRESS THE BUTTON";
            _timeText.text = $"({Mathf.CeilToInt(time)}s)";
            yield return null;

        }

                       
        _timeText.text = _success ? "Success" : "Failed";
        _image.color = _success ? Color.green : Color.red;
        if (!_success) OnFailed();

        yield return new WaitForSeconds(1f);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (_qteActive)
        {
            _success = true;
            _building.PayOut(_type, _addMount);
            _building.CountProduction();
            _qteActive = false;
        }
    }

    private void OnFailed()
    {
        
    }
}
