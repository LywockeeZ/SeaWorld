using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace JiongXiaGu.BuoyancySystems
{

    [CanEditMultipleObjects]
    [CustomEditor(typeof(SphereBuoyancy), true)]
    public class BuoyancyPointDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            SphereBuoyancy.IsDrawWire = EditorGUILayout.Toggle(nameof(SphereBuoyancy.IsDrawWire), SphereBuoyancy.IsDrawWire);
            base.OnInspectorGUI();
        }
    }
}
