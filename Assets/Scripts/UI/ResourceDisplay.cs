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
    [SerializeField] private displayOptions _displayType;
    [SerializeField] private bool _showDisplayName = true;
    [SerializeField] private float _OneByOneDuration = 0.5f;
    [SerializeField] private RollingNumberEffect _rollingNumberEffect;
    
    private enum displayOptions {Instant, OneByOne}

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
            _rollingNumberEffect.TryPlaySFX(true);
        }
        if (_displayType == displayOptions.OneByOne)
        {
            if (_resource != null) _rollingNumberEffect.UpdateText(_resource.Get(_type));
        }

    }
}
