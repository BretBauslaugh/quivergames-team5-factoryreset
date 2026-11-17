using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonAction : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(DoAction);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(DoAction);
        }

        protected abstract void DoAction();
    }
}