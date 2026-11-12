using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTE_PressTheButton : MonoBehaviour
{

    [SerializeField] private bool _buttonPressed;
    [SerializeField] private float _secondsLeft = 5;
    [SerializeField] private TextMeshProUGUI _secondsLeftLabel;
    [SerializeField] private GameObject _parentObject;
    [Header("Debug Only")]
    [SerializeField] private GameObject _InputListener;
    private Image _image;
    private float _timeLeft;
    void OnEnable()
    {
        _timeLeft = _secondsLeft; //_secondsLeft must be unmodified so the GUI can be reused when re-enabled
        _image = GetComponent<Image>();
        _image.color = Color.white;
        _buttonPressed = false;
    }

    private void Update()
    {
        if (!_buttonPressed)
        {
        _timeLeft -= Time.deltaTime;
        int _time_ToInt = Mathf.RoundToInt(_timeLeft);
        if (_time_ToInt != 1) _secondsLeftLabel.text = $"{_time_ToInt.ToString()} SECONDS LEFT";
        else _secondsLeftLabel.text = "1 SECOND LEFT";
        }
        if (_timeLeft < .5 && !_buttonPressed)
        {
        _secondsLeftLabel.text = "FAILED!";
        _image.color = Color.red;
        StartCoroutine(QuickTimeFailed());
        }
        else if (_timeLeft >= .5 && _buttonPressed)
        {
        _secondsLeftLabel.text = "SUCCESS!";
        _image.color = Color.green;
        StartCoroutine(CloseEvent());
        }
    }

    IEnumerator QuickTimeFailed()
    {
        yield return new WaitForSeconds(1);
        Destroy(_parentObject);
        gameObject.SetActive(false); //disable the QTE gameobject
        if (_InputListener != null) _InputListener.SetActive(true); //if debugging, re-enables quick time action debug
    }

    IEnumerator CloseEvent()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false); //disable the QTE gameobject
        if (_InputListener != null) _InputListener.SetActive(true); //if debugging, re-enables quick time action debug
    }

    public void OnPress()
    {
        _buttonPressed = true;
    }
}
