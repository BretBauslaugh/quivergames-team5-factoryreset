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
        TryPause();
    }

    public void TryPause()
    {
        if (_gamePaused)
        {
            onResume?.Invoke();
            _gamePaused = false;
        }
        if (!_gamePaused)
        {
            onPaused?.Invoke();
            _gamePaused = true;
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
