using PrimeTween;
using System.Collections;
using UnityEngine;

public class WarningPopupAnimation : MonoBehaviour
{
    [SerializeField] private float _popupTime;
    [SerializeField] private ShakeSettings _scoreScale;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _parentObject;

    private void Reset()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        StartCoroutine(PlayPopup());
    }

    public void PlayAnimation()
    {
        Tween.PunchScale(_transform, _scoreScale);
    }

    IEnumerator PlayPopup()
    {
        PlayAnimation();
        yield return new WaitForSeconds(_popupTime);
        _parentObject.SetActive(false); //disable game object
    }
}
