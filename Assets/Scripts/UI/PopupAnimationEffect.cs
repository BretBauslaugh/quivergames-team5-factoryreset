using FMOD.Studio;
using FMODUnity;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

public class PopupAnimationEffect : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private ShakeSettings tweenObject;
    [SerializeField] private Transform tweenTransform;
    [Header("FMOD")]
    [SerializeField] private EventReference popupSFX;
    [SerializeField] private bool enableSFX;
    private void Start()
    {
        if (tweenTransform == null) tweenTransform = transform; //if no transform is selected in Inspector, the script would just grab the transform from the GameObject it's attached to
        Tween.PunchScale(tweenTransform, tweenObject);
        if (enableSFX) Playsfx(popupSFX); 
    }
    private void Playsfx(EventReference sfx)
    {
        if (sfx.IsNull)
        {
            Debug.LogError("ERROR: [FMOD] Event not found.");
            return;
        }
        EventInstance instance = RuntimeManager.CreateInstance(sfx);
        instance.set3DAttributes(transform.position.To3DAttributes()); //added to prevent warnings
        RuntimeManager.PlayOneShot(sfx, transform.position);
    }
}
