using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private bool _showDisplayName = true;

    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        if (_showDisplayName) _text.text = $"{_type.DisplayName}: {_resource.Get(_type)}";
        else _text.text = $"{_resource.Get(_type)}";
    }
}
