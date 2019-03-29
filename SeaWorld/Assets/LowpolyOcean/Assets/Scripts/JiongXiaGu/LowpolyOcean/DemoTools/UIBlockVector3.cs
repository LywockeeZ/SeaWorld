using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JiongXiaGu.LowpolyOcean.DemoTools
{

    [DisallowMultipleComponent]
    public class UIBlockVector3 : MonoBehaviour
    {
        protected UIBlockVector3()
        {
        }

        [SerializeField] private Text lable;
        [SerializeField] private UIBlockSlider x;
        [SerializeField] private UIBlockSlider y;
        [SerializeField] private UIBlockSlider z;
        [SerializeField] private OnValueChangedEvent onValueChanged;
        public Text Lable => lable;
        public UIBlockSlider X => x;
        public UIBlockSlider Y => y;
        public UIBlockSlider Z => z;
        public OnValueChangedEvent OnValueChanged => onValueChanged;

        protected virtual void Start()
        {
            x.OnValueChanged.AddListener(OnAnyValueChanged);
            y.OnValueChanged.AddListener(OnAnyValueChanged);
            z.OnValueChanged.AddListener(OnAnyValueChanged);
        }

        protected virtual void OnAnyValueChanged(float value)
        {
            Vector3 vector = GetValue();
            OnValueChanged.Invoke(vector);
        }

        public virtual void SetValue(Vector3 vector)
        {
            x.SetValue(vector.x);
            y.SetValue(vector.y);
            z.SetValue(vector.z);
        }

        public virtual Vector3 GetValue()
        {
            Vector3 vector = new Vector3()
            {
                x = x.GetValue(),
                y = y.GetValue(),
                z = z.GetValue(),
            };
            return vector;
        }

        [Serializable]
        public class OnValueChangedEvent : UnityEvent<Vector3>
        {
        }
    }
}
