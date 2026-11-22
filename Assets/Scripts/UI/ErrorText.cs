using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ErrorText : MonoBehaviour
{
    public static ErrorText Instance { get; private set; }

    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _warningObject;
    [SerializeField] private float _showTime = 1f;
    [SerializeField] private ShakeSettings _animation;
    [SerializeField] private Transform _animationTransform;

    private Coroutine _hide;

    private void Awake()
    {
        Instance = this;
        _warningObject.gameObject.SetActive(false);
    }

    public static void Show(string mesg)
    {
        if (Instance != null) Instance.SetupText(mesg);
        else Debug.LogWarning(mesg);
    }

    public void SetupText(string mesg)
    {
        _text.text = mesg;
        Tween.PunchScale(_animationTransform, _animation);
        _warningObject.gameObject.SetActive(true);

        if(_hide != null) StopCoroutine(_hide);
        _hide = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(_showTime);
        _warningObject.gameObject.SetActive(false);
    }
}
