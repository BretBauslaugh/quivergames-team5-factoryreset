using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    public class SetSliderValue : MonoBehaviour
    {
        [SerializeField] private FloatSetting _setting;

        private Slider _slider;

        private void OnEnable()
        {
            _slider = GetComponent<Slider>();
            _slider.SetValueWithoutNotify(_setting.GetSliderValue());

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            _setting.SetValueFromSlider(sliderValue);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}