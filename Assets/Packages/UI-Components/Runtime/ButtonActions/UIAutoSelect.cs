using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIComponents
{
    [DisallowMultipleComponent]
    public class UIAutoSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, IPointerClickHandler
    {
        [System.Serializable]
        private class TextOptions
        {
            public Color Color = Color.white;
            public FontStyles Style;

            public TextOptions(Color color, FontStyles style)
            {
                Color = color;
                Style = style;
            }
        }

        [Header("Behaviour")]
        [SerializeField] private bool _firstSelected = false;

        [Header("Color Change")]
        [SerializeField] private Image _imageColorChange;
        [SerializeField] private Color _deselectedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
        [SerializeField] private Color _selectedColor = Color.white;

        [Header("Text Change")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextOptions _normalOptions = new TextOptions(new Color(0.75f, 0.75f, 0.75f, 1f), FontStyles.Normal | FontStyles.UpperCase);
        [SerializeField] private TextOptions _selectOptions = new TextOptions(Color.white, FontStyles.Bold | FontStyles.UpperCase);

#if FMOD
        [Header("FMOD Audio")]
        [SerializeField] private FMODUnity.EventReference _selectEvent;
        [SerializeField] private FMODUnity.EventReference _clickEvent;
        [SerializeField] private FMODUnity.EventReference _moveEvent;
#endif

        private void OnEnable()
        {
            SetSelectedColor(false);
            SetTextStyle(false);
            if (_firstSelected) SelectElement();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            SelectElement();
        }

        private void SelectElement()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnSelect(BaseEventData eventData)
        {
#if FMOD
            if (!_selectEvent.IsNull) FMODUnity.RuntimeManager.PlayOneShot(_selectEvent);
            SetSelectedColor(true);
            SetTextStyle(true);
#endif
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetSelectedColor(false);
            SetTextStyle(false);
        }

        public void OnMove(AxisEventData eventData)
        {
#if FMOD
            if (!_moveEvent.IsNull) FMODUnity.RuntimeManager.PlayOneShot(_moveEvent);
#endif
        }

        public void OnSubmit(BaseEventData eventData)
        {
#if FMOD
            if (!_clickEvent.IsNull) FMODUnity.RuntimeManager.PlayOneShot(_clickEvent);
#endif
        }

        public void OnPointerClick(PointerEventData eventData)
        {
#if FMOD
            if (!_clickEvent.IsNull) FMODUnity.RuntimeManager.PlayOneShot(_clickEvent);
#endif
        }

        private void SetSelectedColor(bool isSelected)
        {
            if (_imageColorChange == null) return;
            _imageColorChange.color = isSelected ? _selectedColor : _deselectedColor;
        }

        private void SetTextStyle(bool isSelected)
        {
            if (_text == null) return;
            _text.color = isSelected ? _selectOptions.Color : _normalOptions.Color;
            _text.fontStyle = isSelected ? _selectOptions.Style : _normalOptions.Style;
        }
    }
}