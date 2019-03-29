using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [CustomPropertyDrawer(typeof(MaterialData))]
    public sealed class MaterialDataDrawer : PropertyDrawer
    {
        private SerializedProperty GetProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(nameof(MaterialData.Material));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, GetProperty(property), label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetProperty(property), label);
        }
    }
}
