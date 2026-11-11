using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        Refresh();
    }


    public void Refresh()
    {
        _text.text = $"{_type.DisplayName}: {_resource.Get(_type)}";
    }
}
