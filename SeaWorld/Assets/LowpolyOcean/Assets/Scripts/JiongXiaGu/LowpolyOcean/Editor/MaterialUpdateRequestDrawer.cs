using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [CustomPropertyDrawer(typeof(MaterialUpdateRequest))]
    public class MaterialUpdateRequestDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty name = property.FindPropertyRelative(nameof(MaterialUpdateRequest.Name));
            if (!string.IsNullOrWhiteSpace(name.stringValue))
            {
                label = new GUIContent(name.stringValue);
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
