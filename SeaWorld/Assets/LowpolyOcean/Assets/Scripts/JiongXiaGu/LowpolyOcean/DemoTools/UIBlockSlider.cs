using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace JiongXiaGu.LowpolyOcean.DemoTools
{

    [DisallowMultipleComponent]
    public class UIBlockSlider : MonoBehaviour
    {
        protected UIBlockSlider()
        {
        }

        [SerializeField] private Text lable;
        [SerializeField] private Slider slider;
        [SerializeField] private InputField inputField;
        public Text Lable => lable;
        public Slider Slider => slider;
        public InputField InputField => inputField;
        public UnityEvent<float> OnValueChanged => Slider.onValueChanged;

        protected virtual void Start()
        {
            if (Slider == null)
                return;

            Slider.onValueChanged.AddListener(OnSliderValueChanged);
            OnSliderValueChanged(slider.value);
        }

        protected virtual void OnSliderValueChanged(float value)
        {
            inputField.text = value.ToString();
        }

        public virtual void SetValue(string lable, float value, float minValue, float maxValue)
        {
            if (lable != null)
            {
                this.lable.text = lable;
            }

            Slider.minValue = minValue;
            Slider.maxValue = maxValue;
            Slider.value = value;
        }

        public virtual void SetValue(float value)
        {
            Slider.value = value;
        }

        public virtual float GetValue()
        {
            return Slider.value;
        }
    }
}
