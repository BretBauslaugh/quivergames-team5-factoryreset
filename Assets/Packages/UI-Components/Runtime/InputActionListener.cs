using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UIComponents
{
    public class InputActionListener : MonoBehaviour
    {
        [SerializeField] private InputActionReference _action;
        [SerializeField] private Button _activateButton;
        [SerializeField] private float _enabledInputDelay = 0.25f;

        public UnityEvent OnPerformed;

        private float _enabledTime;

        private void OnEnable()
        {
            _enabledTime = Time.unscaledTime;
            if(_action != null) _action.action.performed += Performed;
        }

        private void OnDisable()
        {
            if(_action != null) _action.action.performed -= Performed;
        }

        private void Performed(InputAction.CallbackContext ctx)
        {
            if (Time.unscaledTime < _enabledTime + _enabledInputDelay) return;
            OnPerformed.Invoke();
            _activateButton?.onClick.Invoke();
        }

        public void ForcePerform()
        {
            OnPerformed.Invoke();
        }
    }
}