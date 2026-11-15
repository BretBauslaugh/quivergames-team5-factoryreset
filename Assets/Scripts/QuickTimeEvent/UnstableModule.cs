using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UnstableModule : MonoBehaviour
{
    [SerializeField] private float _minTime = 10f;
    [SerializeField] private float _maxTime = 20f;
    [SerializeField] private float _timeLimit = 5f;
    [SerializeField] private int _buttonPress = 3;

    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, 100f);

    private int _pressed = 0;
    private bool _qteActive = false;
    private Camera _cam;

    private void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(RandomTrigger());
    }

    private IEnumerator RandomTrigger()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minTime, _maxTime));
            yield return RunQTE();
        }
    }

    private IEnumerator RunQTE()
    {
        _image.color = Color.white;
        _pressed = 0;
        _qteActive = true;
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

        yield return new WaitForSeconds(1f);
        _ui.SetActive(false); 
    }

    public void OnPress()
    {
        if (_qteActive) _pressed++;
    }

    private void OnFailed()
    {
        //Explosion
    }





}
