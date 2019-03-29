using JiongXiaGu.ShaderTools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.LowpolyOcean
{

    [CustomPropertyDrawer(typeof(OceanShaderOptions))]
    public class OceanShaderOptionsDrawer : PropertyDrawer
    {
        private List<KeyValuePair<string, IDrawer>> customDrawer;
        private KeywordDrawer keywordDrawer;
        private IDrawer[] drawers;
        private SerializedProperty Cull;
        private static OceanMode alwaysEnableMode = OceanMode.Tessellation;

        private static float singleLineHeight => EditorGUIUtility.singleLineHeight;

        private void Init(SerializedProperty properties)
        {
            if (keywordDrawer != null)
                return;

            keywordDrawer = new KeywordDrawer(properties);
            alwaysEnableMode |= OceanMode.Tessellation;

            drawers = new IDrawer[]
            {
                new DefaultDrawer(properties, OceanShaderOptions.TessellationAccessor, nameof(OceanShaderOptions.Tessellation)),
                new WaveDrawer(properties),
                new DefaultDrawer(properties, OceanShaderOptions.LightingAccessor, nameof(OceanShaderOptions.Lighting)),
                new DefaultDrawer(properties, OceanShaderOptions.RefractionAccessor, nameof(OceanShaderOptions.Refraction)),
                new DefaultDrawer(properties, OceanShaderOptions.FoamAccessor, nameof(OceanShaderOptions.Foam)),
                new DefaultDrawer(properties, OceanShaderOptions.CookieAccessor, nameof(OceanShaderOptions.Cookie)),
                new DefaultDrawer(properties, OceanShaderOptions.ReflectionAccessor, nameof(OceanShaderOptions.Reflection)),
                new DefaultDrawer(properties, OceanShaderOptions.BackLightingAccessor, nameof(OceanShaderOptions.BackLighting)),
                new DefaultDrawer(properties, OceanShaderOptions.BackRefractionAccessor, nameof(OceanShaderOptions.BackRefraction)),
                new DefaultDrawer(properties, OceanShaderOptions.PointLightingAccessor, nameof(OceanShaderOptions.PointLighting)),
            };

            Cull = properties.FindPropertyRelative(nameof(OceanShaderOptions.Cull));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);
            float height = singleLineHeight;
            if (property.isExpanded)
            {
                height += keywordDrawer.GetPropertyHeight();

                var mode = keywordDrawer.Extract();
                mode |= alwaysEnableMode;

                foreach (var drawer in drawers)
                {
                    height += drawer.GetPropertyHeight(mode);
                }

                height += singleLineHeight;
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init(property);

            position.height = singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName);
            AddOneLine(ref position);
            if (property.isExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    keywordDrawer.OnGUI(position);
                    position.y += keywordDrawer.GetPropertyHeight();

                    var mode = keywordDrawer.Extract();
                    mode |= alwaysEnableMode;

                    foreach (var drawer in drawers)
                    {
                        drawer.OnGUI(position, mode);
                        position.y += drawer.GetPropertyHeight(mode);
                    }
                }

                EditorGUI.PropertyField(position, Cull);
            }
        }

        private static void AddOneLine(ref Rect position)
        {
            position.y += singleLineHeight;
        }

        public interface IDrawer
        {
            SerializedProperty SerializedProperty { get; }
            float GetPropertyHeight(OceanMode mask);
            void OnGUI(Rect position, OceanMode mask);
        }

        private class DefaultDrawer : IDrawer
        {
            public SerializedProperty SerializedProperty { get; }
            private SerializedPropertyDrawer autoDrawer;

            public DefaultDrawer(SerializedProperty property, IShaderFieldGroup group, string name)
            {
                SerializedProperty = property = property.FindPropertyRelative(name);
                autoDrawer = new SerializedPropertyDrawer(group, property);
            }

            public float GetPropertyHeight(OceanMode mask)
            {
                if ((autoDrawer.Group.Mask & (int)mask) == 0)
                    return 0;

                float height = singleLineHeight;
                if (SerializedProperty.isExpanded)
                {
                    height += autoDrawer.GetPropertyHeight((int)mask);
                }
                return height;
            }

            public void OnGUI(Rect position, OceanMode mask)
            {
                if ((autoDrawer.Group.Mask & (int)mask) == 0)
                    return;

                SerializedProperty.isExpanded = EditorGUI.Foldout(position, SerializedProperty.isExpanded, SerializedProperty.displayName);
                AddOneLine(ref position);
                if (SerializedProperty.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        autoDrawer.OnGUI(position, (int)mask);
                    }
                }
            }
        }

        private class KeywordDrawer
        {
            public SerializedProperty SerializedProperty { get; }
            private SerializedPropertyDrawer autoDrawer;

            public KeywordDrawer(SerializedProperty property)
            {
                SerializedProperty = property = property.FindPropertyRelative(nameof(OceanShaderOptions.Mode));
                autoDrawer = new SerializedPropertyDrawer(OceanShaderOptions.ModeAccessor, property);
            }

            public float GetPropertyHeight()
            {
                float height = singleLineHeight;
                if (SerializedProperty.isExpanded)
                {
                    height += autoDrawer.GetPropertyHeight(~0);
                }
                return height;
            }

            public void OnGUI(Rect position)
            {
                SerializedProperty.isExpanded = EditorGUI.Foldout(position, SerializedProperty.isExpanded, SerializedProperty.displayName);
                AddOneLine(ref position);
                if (SerializedProperty.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        autoDrawer.OnGUI(position, ~0);
                    }
                }
            }

            public OceanMode Extract()
            {
                autoDrawer.Extract();
                return (OceanMode)autoDrawer.Mask;
            }
        }

        private class WaveDrawer : IDrawer
        {
            public SerializedProperty SerializedProperty { get; }
            private SerializedProperty Texture;
            private SerializedProperty[] Rect;
            private SerializedProperty Radian;
            private SerializedProperty HeightPow;
            private SerializedProperty HeightScale;
            private SerializedProperty SpeedZ;

            public WaveDrawer(SerializedProperty property)
            {
                SerializedProperty = property = property.FindPropertyRelative(nameof(OceanShaderOptions.Wave));
                Texture = property.FindPropertyRelative(nameof(WaveOptions.Texture));
                Rect = new SerializedProperty[4];
                Rect[0] = property.FindPropertyRelative(nameof(WaveOptions.Rect0));
                Rect[1] = property.FindPropertyRelative(nameof(WaveOptions.Rect1));
                Rect[2] = property.FindPropertyRelative(nameof(WaveOptions.Rect2));
                Rect[3] = property.FindPropertyRelative(nameof(WaveOptions.Rect3));
                Radian = property.FindPropertyRelative(nameof(WaveOptions.Radian));
                HeightPow = property.FindPropertyRelative(nameof(WaveOptions.HeightPow));
                HeightScale = property.FindPropertyRelative(nameof(WaveOptions.HeightScale));
                SpeedZ = property.FindPropertyRelative(nameof(WaveOptions.SpeedZ));
            }

            private void SliderVector3(Rect position, string name, SerializedProperty serializedProperty, int index, float leftValue, float rightValue)
            {
                EditorGUI.BeginChangeCheck();
                var value = serializedProperty.vector3Value;
                var newValue = EditorGUI.Slider(position, name, value[index], leftValue, rightValue);
                if (EditorGUI.EndChangeCheck())
                {
                    value[index] = newValue;
                    serializedProperty.vector3Value = value;
                }
            }

            private void SliderVector4(Rect position, string name, SerializedProperty serializedProperty, int index, float leftValue, float rightValue)
            {
                var value = serializedProperty.vector4Value;
                var newValue = EditorGUI.Slider(position, name, value[index], leftValue, rightValue);
                if (value[index] != newValue)
                {
                    value[index] = newValue;
                    serializedProperty.vector4Value = value;
                }
            }

            private void SliderRadian(Rect position, string name, SerializedProperty serializedProperty, int index, float leftValue, float rightValue)
            {
                var value = serializedProperty.vector4Value;
                var radian = value[index];
                var angle = radian * MathfHelper.RadinaToAngle;
                var newAngle = EditorGUI.Slider(position, name, angle, leftValue, rightValue);
                if (angle != newAngle)
                {
                    radian = newAngle * MathfHelper.AngleToRadina;
                    value[index] = radian;
                    serializedProperty.vector4Value = value;
                }
            }

            private void DrawWaveOptions(ref Rect position, string name, int index)
            {
                position.y += singleLineHeight;
                Rect[index].isExpanded = EditorGUI.Foldout(position, Rect[index].isExpanded, name);
                if (Rect[index].isExpanded)
                {
                    position.y += singleLineHeight;
                    Rect[index].vector4Value = EditorGUI.Vector4Field(position, WaveOptionsDrawData.RectDisplayName, Rect[index].vector4Value);

                    position.y += singleLineHeight;
                    SliderRadian(position, WaveOptionsDrawData.AngleDisplayName, Radian, index, WaveOptionsDrawData.AngleLeftValue, WaveOptionsDrawData.AngleRightValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.HeightPowDisplayName, HeightPow, index, WaveOptionsDrawData.HeightMinValue, WaveOptionsDrawData.HeightMaxValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.HeightScaleDisplayName, HeightScale, index, WaveOptionsDrawData.HeightMinValue, WaveOptionsDrawData.HeightMaxValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.SpeedZDisplayName, SpeedZ, index, WaveOptionsDrawData.SpeedMinValue, WaveOptionsDrawData.SpeedMaxValue);
                }
            }

            private void DrawWaveHorizontalOffset(ref Rect position, string name, int index)
            {
                position.y += singleLineHeight;
                Rect[index].isExpanded = EditorGUI.Foldout(position, Rect[index].isExpanded, name);
                if (Rect[index].isExpanded)
                {
                    AddOneLine(ref position);
                    Rect[index].vector4Value = EditorGUI.Vector4Field(position, WaveOptionsDrawData.RectDisplayName, Rect[index].vector4Value);

                    AddOneLine(ref position);
                    SliderRadian(position, WaveOptionsDrawData.AngleDisplayName, Radian, index, WaveOptionsDrawData.AngleLeftValue, WaveOptionsDrawData.AngleRightValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.HorizontalOffsetXDisplayName, HeightPow, index, WaveOptionsDrawData.HeightMinValue, WaveOptionsDrawData.HeightMaxValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.HorizontalOffsetYDisplayName, HeightScale, index, WaveOptionsDrawData.HeightMinValue, WaveOptionsDrawData.HeightMaxValue);

                    position.y += singleLineHeight;
                    SliderVector4(position, WaveOptionsDrawData.SpeedZDisplayName, SpeedZ, index, WaveOptionsDrawData.SpeedMinValue, WaveOptionsDrawData.SpeedMaxValue);
                }
            }

            public void OnGUI(Rect position, OceanMode mask)
            {
                if ((mask & WaveOptions.WaveAll) == 0)
                    return ;

                position.height = singleLineHeight;
                SerializedProperty.isExpanded = EditorGUI.Foldout(position, SerializedProperty.isExpanded, nameof(OceanShaderOptions.Wave));
                if (SerializedProperty.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        position.y += singleLineHeight;
                        EditorGUI.PropertyField(position, Texture);

                        if ((mask & OceanMode.Wave1) != 0)
                        {
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave1Names[0], 2);
                            DrawWaveHorizontalOffset(ref position, WaveOptionsDrawData.Wave1Names[1], 3);
                        }
                        else if ((mask & OceanMode.Wave2) != 0)
                        {
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave2Names[0], 0);
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave2Names[1], 1);
                        }
                        else if ((mask & OceanMode.Wave3) != 0)
                        {
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave4Names[0], 0);
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave4Names[1], 1);
                            DrawWaveOptions(ref position, WaveOptionsDrawData.Wave4Names[2], 2);
                            DrawWaveHorizontalOffset(ref position, WaveOptionsDrawData.Wave4Names[3], 3);
                        }
                    }
                }
            }

            private float GetWaveOptionsHeight(int index)
            {
                float height = 0;
                height += singleLineHeight;

                if (Rect[index].isExpanded)
                {
                    height += singleLineHeight * 5;
                }

                return height;
            }

            private float GetWaveHorizontalHeight(int index)
            {
                return GetWaveOptionsHeight(index);
            }

            public float GetPropertyHeight(OceanMode mask)
            {
                if ((mask & WaveOptions.WaveAll) == 0)
                    return 0;

                float height = singleLineHeight;

                if (SerializedProperty.isExpanded)
                {
                    height += singleLineHeight * 1;

                    if ((mask & OceanMode.Wave1) != 0)
                    {
                        height += GetWaveOptionsHeight(2);
                        height += GetWaveHorizontalHeight(3);
                    }
                    else if ((mask & OceanMode.Wave2) != 0)
                    {
                        height += GetWaveOptionsHeight(0);
                        height += GetWaveHorizontalHeight(1);
                    }
                    else if ((mask & OceanMode.Wave3) != 0)
                    {
                        height += GetWaveOptionsHeight(0);
                        height += GetWaveOptionsHeight(1);
                        height += GetWaveOptionsHeight(2);
                        height += GetWaveOptionsHeight(3);
                    }
                }

                return height;
            }
        }
    }
}
