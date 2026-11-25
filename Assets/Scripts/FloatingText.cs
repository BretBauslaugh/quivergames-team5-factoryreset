using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _lifetime = 1.2f;
    [SerializeField] private float _riseSpeed = 1.5f;

    private float _age;
    private Color _color;
    private Camera _cam;

    public void Show(string msg, Color color)
    {
        _text.text = msg;
        _color = color;
        _text.color = color;
        _cam = Camera.main;
    }

    void LateUpdate()
    {
        _age += Time.deltaTime;
        transform.position += Vector3.up * _riseSpeed * Time.deltaTime;
        if (_cam) transform.rotation = _cam.transform.rotation;

        var c = _color;
        c.a = 1f - Mathf.Clamp01((_age / _lifetime - 0.5f) * 2f);
        _text.color = c;

        if (_age >= _lifetime) Destroy(gameObject);
    }
}