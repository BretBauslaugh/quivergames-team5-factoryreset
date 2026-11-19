using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UnityEvent onPaused;
    [SerializeField] private UnityEvent onResume;
    private bool _gamePaused;
    public void OnPause(InputValue value)
    {
        if(!value.isPressed) return;
        TryPause();
    }

    public void TryPause()
    {
        if (_gamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        onResume?.Invoke();
        _gamePaused = false;
    }
    public void Pause()
    {
        onPaused?.Invoke();
        _gamePaused = true;
    }
}
