using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private EventReference _resourceCollectSFX;
    [SerializeField] private displayOptions _displayType;
    [SerializeField] private bool _showDisplayName = true;
    [SerializeField] private float _OneByOneDuration = 0.5f;
    private enum displayOptions {Instant, OneByOne}
    private Coroutine _countingCoroutine;
    private int _currentDisplayedValue;
    private int _targetValue;

    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        if (_displayType == displayOptions.Instant)
        {
            if (_showDisplayName) _text.text = $"{_type.DisplayName}: {_resource.Get(_type)}";
            else _text.text = $"{_resource.Get(_type)}";  
            if (!_resourceCollectSFX.IsNull) PlaySFX(_resourceCollectSFX);
        }
        if (_displayType == displayOptions.OneByOne)
        {
            CountText(10);
        }

    }


    private IEnumerator CountText(int targetValue)
    {
        int startValue = _currentDisplayedValue;
        float elapsed = 0f;

        while (elapsed < _OneByOneDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / _OneByOneDuration);

            _currentDisplayedValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, progress));
            _text.SetText(_currentDisplayedValue.ToString("N0"));
            if (!_resourceCollectSFX.IsNull) PlaySFX(_resourceCollectSFX);
            yield return null;
        }

        _currentDisplayedValue = targetValue;
        _text.SetText(_currentDisplayedValue.ToString("N0"));
        _countingCoroutine = null;
    }
    public void PlaySFX(EventReference SFX)
    {
        EventInstance instance = RuntimeManager.CreateInstance(SFX);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position)); //added to prevent warnings
        RuntimeManager.PlayOneShot(SFX, transform.position);
    }
}
