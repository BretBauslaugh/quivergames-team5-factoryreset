using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;

public class RollingNumberEffect : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private string _numberFormat = "N0";
    [SerializeField] private string _prefix = "$ ";
    [SerializeField] private EventReference _scoreAdd;
    [SerializeField] private EventReference _scoreRemove;
    [SerializeField] private bool _disableSFX;

    private TextMeshProUGUI _text;
    private Coroutine _countingCoroutine;
    private int _currentDisplayedValue;
    private int _targetValue;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int newValue)
    {

        if (newValue == _targetValue) return;

        _targetValue = newValue;

        if (gameObject.activeInHierarchy)
        {
            if (_countingCoroutine != null)
            {
                StopCoroutine(_countingCoroutine);
            }
            _countingCoroutine = StartCoroutine(CountText(newValue));
        }
    }

    private IEnumerator CountText(int targetValue)
    {
        int startValue = _currentDisplayedValue;
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / _duration);
            int current = _currentDisplayedValue; //this makes sure that SFX does NOT play if the value is not different. Prevents ticking SFX playing more than actual value changing.
                                                  //this also checks if the value is increasing or decreasing, so we can check if the player is spending or gaining resources.
            
            _currentDisplayedValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, progress));
            if (_text != null) _text.SetText(_prefix + _currentDisplayedValue.ToString(_numberFormat));
            if (current != _currentDisplayedValue && !_disableSFX)
            {
                if (current > _currentDisplayedValue) TryPlaySFX(false);
                if (current < _currentDisplayedValue) TryPlaySFX(true);
            }
            yield return null;
        }
        _currentDisplayedValue = targetValue;
        _text.SetText(_prefix + _currentDisplayedValue.ToString(_numberFormat));
        _countingCoroutine = null;
    }

    public void TryPlaySFX(bool isAdding)
    {
        if (isAdding && !_disableSFX) PlaySFX(_scoreAdd);
        if (!isAdding && !_disableSFX) PlaySFX(_scoreRemove);
    }

    private void PlaySFX(EventReference SFX)
    {
        if (!SFX.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(SFX);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            instance.start();
            instance.release();
        }
        else Debug.LogError("ERROR: [FMOD] Event not found.");
    }

}