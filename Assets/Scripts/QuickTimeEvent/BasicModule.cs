using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class BasicModule : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private ResourceType _type;
    [SerializeField] private float _time;
    [SerializeField] private int _addMount;

    [SerializeField] GameObject _ui;
    [SerializeField] private TMP_Text _labelText;
    [SerializeField] private Image _image;
    [SerializeField] private Vector2 _screenOffset = new Vector2(0f, 200f);

    private bool _qteActive = false;
    private Camera _cam;

    void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Preview"))
        {
            enabled = false;
            return;
        }

        _ui.SetActive(false);
        _cam = Camera.main;
        _image.color = Color.white;
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_time);
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

            _ui.transform.position = _cam.WorldToScreenPoint(transform.position) + (Vector3)_screenOffset;
            _labelText.text = $"PRESS THE BUTTON";
            yield return null;

        }              
        
        _image.color = Color.green;        
        yield return new WaitForSeconds(1f);
        _ui.SetActive(false);
    }

    public void OnPress()
    {
        if (_qteActive) 
        {
            _resource.Add(_type, _addMount);
            _qteActive = false;
        }
    }

}
