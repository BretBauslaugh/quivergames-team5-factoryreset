using UnityEngine;
using UnityEngine.InputSystem;

#if FMOD
using FMOD.Studio;
using FMODUnity;
#endif

namespace UIComponents
{
    public class PauseBehaviours : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private float _timeScale = 1f;
        [SerializeField] private string _actionMapName = "Player";
        [SerializeField] private CursorLockMode _cursorLockMode = CursorLockMode.Locked;

        [Header("Hierarchy Control")]
        [SerializeField] private GameObject[] _toDisable;
        [SerializeField] private GameObject[] _toEnable;

#if FMOD
        [Header("FMOD")]
        [SerializeField, ParamRef] private string _gameStateParameter;
        [SerializeField] private float _gameStateValue = 0f;
#endif

        public void DoAll()
        {
            Time.timeScale = _timeScale;
            SetAllActionMaps(_actionMapName);
            Cursor.lockState = _cursorLockMode;

            foreach (GameObject go in _toDisable)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in _toEnable)
            {
                go.SetActive(true);
            }

            if (!string.IsNullOrEmpty(_gameStateParameter))
            {
                SetGlobalFMODParameter(_gameStateParameter, _gameStateValue);
            }
        }

        private void SetAllActionMaps(string mapName)
        {
            PlayerInput[] inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
            foreach (PlayerInput input in inputs)
            {
                input.SwitchCurrentActionMap(mapName);
            }
        }

        private void SetGlobalFMODParameter(string parameterName, float value)
        {
#if FMOD
            FMOD.RESULT result = RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
            if (result != FMOD.RESULT.OK)
            {
                RuntimeUtils.DebugLogError(string.Format(("[FMOD] StudioGlobalParameterTrigger failed to set parameter {0} : result = {1}"), parameterName, result));
                return;
            }
#endif
        }
    }
}