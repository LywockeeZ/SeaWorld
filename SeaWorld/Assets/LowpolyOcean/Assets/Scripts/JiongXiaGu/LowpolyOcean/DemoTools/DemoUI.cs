using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JiongXiaGu.LowpolyOcean.DemoTools
{

    [DisallowMultipleComponent]
    public class DemoUI : MonoBehaviour
    {
        public OceanExpandRender oceanRender;
        public OceanGeneralSettings settings;
        public Light sun;
        public UIBlockSlider wavePowUI;
        public UIBlockSlider waveHeightUI;
        public UIBlockSlider waveSpeedUI;
        public UIBlockSlider clarityUI;
        public UIBlockSlider sunIntensityUI;
        public UIBlockVector3 sunDirectionUI;
        public Button hiddenOceanBtn;

        private void Start()
        {
            if (oceanRender == null)
                throw new ArgumentNullException(nameof(settings));

            OceanShaderOptions options = settings.ShaderOptions;

            wavePowUI.SetValue("Wave pow", options.Wave.HeightPow.z, -5, 5);
            AddListener(wavePowUI, value => options.Wave.HeightPow.z = value);

            waveHeightUI.SetValue("Wave height", options.Wave.HeightScale.z, -10, 10);
            AddListener(waveHeightUI, value => options.Wave.HeightScale.z = value);

            waveSpeedUI.SetValue("Wave speed", options.Wave.SpeedZ.z, -0.1f, 0.1f);
            AddListener(waveSpeedUI, value => options.Wave.SpeedZ.z = value);

            clarityUI.SetValue("Clarity", options.Refraction.Clarity, 0, 2f);
            AddListener(clarityUI, value => options.Refraction.Clarity = value);

            sunIntensityUI.Lable.text = "Sun intensity";
            sunIntensityUI.Slider.minValue = 0.001f;
            sunIntensityUI.Slider.maxValue = 2f;
            sunIntensityUI.SetValue(sun.intensity);
            sunIntensityUI.OnValueChanged.AddListener(value => sun.intensity = value);

            sunDirectionUI.Lable.text = "Sun direction";
            sunDirectionUI.X.Slider.minValue = 10;
            sunDirectionUI.X.Slider.maxValue = 170;
            sunDirectionUI.Y.Slider.minValue = 0;
            sunDirectionUI.Y.Slider.maxValue = 360;
            sunDirectionUI.SetValue(sun.transform.eulerAngles);
            sunDirectionUI.OnValueChanged.AddListener(value => sun.transform.eulerAngles = value);

            hiddenOceanBtn.onClick.AddListener(() => oceanRender.gameObject.SetActive(!oceanRender.isActiveAndEnabled));
        }

        private void AddListener(UIBlockSlider ui, UnityAction<float> action)
        {
            ui.OnValueChanged.AddListener(delegate (float value)
            {
                action(value);
                settings.SendValuesChanged();
            });
        }
    }
}
