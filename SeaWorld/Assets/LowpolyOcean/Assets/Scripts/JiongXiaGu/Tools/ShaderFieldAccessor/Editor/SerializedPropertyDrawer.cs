using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JiongXiaGu.ShaderTools
{

    public class SerializedPropertyDrawer
    {
        public IShaderFieldGroup Group { get; }
        private List<Drawer> drawers;
        public int Mask { get; private set; }

        public SerializedPropertyDrawer(IShaderFieldGroup group, SerializedProperty property)
        {
            Group = group;
            drawers = new List<Drawer>(group.Children.Count);
            foreach (var child in group.Children)
            {
                if (child is ShaderField)
                {
                    var field = (ShaderField)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new ShaderFieldDrawer(target, field);
                        drawers.Add(drawer);
                    }
                }
                else if (child is ShaderKeyword)
                {
                    var field = (ShaderKeyword)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new KeywordDrawer(target, field);
                        drawers.Add(drawer);
                    }
                }
                else if (child is ShaderEnumKeyword)
                {
                    var field = (ShaderEnumKeyword)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new EnumKeywordDrawer(target, field);
                        drawers.Add(drawer);
                    }
                }
                else if (child is ShaderEnumFlagsKeyword)
                {
                    var field = (ShaderEnumFlagsKeyword)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new EnumFlagsKeywordDrawer(target, field);
                        drawers.Add(drawer);
                    }
                }
                else if (child is ShaderFieldMark)
                {
                    var field = (ShaderFieldMark)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new Drawer(target);
                        drawers.Add(drawer);
                    }
                }
                else if (child is ShaderCustomField)
                {
                    var field = (ShaderCustomField)child;
                    var target = property.FindPropertyRelative(field.ReflectiveField.Name);
                    if (target != null)
                    {
                        var drawer = new Drawer(target);
                        drawers.Add(drawer);
                    }
                }
            }
        }

        public void OnGUI(int mask)
        {
            foreach (var drawer in drawers)
            {
                drawer.Draw(mask);
            }
        }

        public void OnGUI(Rect position, int mask)
        {
            foreach (var drawer in drawers)
            {
                drawer.Draw(position, mask);
                position.y += drawer.GetPropertyHeight(mask);
            }
        }

        public float GetPropertyHeight(int mask)
        {
            float height = 0;
            foreach (var drawer in drawers)
            {
                height += drawer.GetPropertyHeight(mask);
            }
            return height;
        }

        public void Extract()
        {
            Mask = 0;

            foreach (var drawer in drawers)
            {
                drawer.Extract(this);
            }
        }

        public class Drawer
        {
            public SerializedProperty SerializedProperty { get; }

            public Drawer(SerializedProperty serializedProperty)
            {
                SerializedProperty = serializedProperty;
            }

            public virtual float GetPropertyHeight(int mask)
            {
                return EditorGUI.GetPropertyHeight(SerializedProperty, true);
            }

            public virtual void Draw(Rect position, int mask)
            {
                EditorGUI.PropertyField(position, SerializedProperty, true);
            }

            public virtual void Draw(int mask)
            {
                EditorGUILayout.PropertyField(SerializedProperty);
            }

            public virtual void Extract(SerializedPropertyDrawer parent)
            {
            }
        }

        public class ShaderFieldDrawer : Drawer
        {
            public ShaderFieldBase ShaderField { get; }

            public ShaderFieldDrawer(SerializedProperty serializedProperty, ShaderFieldBase shaderField) : base(serializedProperty)
            {
                ShaderField = shaderField;
            }

            public override float GetPropertyHeight(int mask)
            {
                if ((mask & ShaderField.Mask) != 0)
                {
                    return base.GetPropertyHeight(mask);
                }
                else
                {
                    return 0;
                }
            }

            public override void Draw(Rect position, int mask)
            {
                if ((mask & ShaderField.Mask) != 0)
                {
                    base.Draw(position, mask);
                }
            }

            public override void Draw(int mask)
            {
                if ((mask & ShaderField.Mask) != 0)
                {
                    base.Draw(mask);
                }
            }
        }

        public class KeywordDrawer : ShaderFieldDrawer
        {
            public ShaderKeyword ShaderKeyword { get; }

            public KeywordDrawer(SerializedProperty serializedProperty, ShaderKeyword shaderKeyword) : base(serializedProperty, shaderKeyword)
            {
                ShaderKeyword = shaderKeyword;
            }

            public override void Extract(SerializedPropertyDrawer parent)
            {
                if (SerializedProperty.boolValue)
                {
                    parent.Mask |= ShaderKeyword.Mask;
                }
            }
        }

        public class EnumKeywordDrawer : ShaderFieldDrawer
        {
            public EnumKeywordDrawer(SerializedProperty serializedProperty, ShaderEnumKeyword field) : base(serializedProperty, field)
            {
            }

            public override void Extract(SerializedPropertyDrawer parent)
            {
                if (SerializedProperty.intValue >= 0)
                {
                    parent.Mask |= SerializedProperty.intValue;
                }
            }
        }

        public class EnumFlagsKeywordDrawer : ShaderFieldDrawer
        {
            public EnumFlagsKeywordDrawer(SerializedProperty serializedProperty, ShaderEnumFlagsKeyword field) : base(serializedProperty, field)
            {
            }

            public override void Extract(SerializedPropertyDrawer parent)
            {
                if (SerializedProperty.intValue >= 0)
                {
                    parent.Mask |= SerializedProperty.intValue;
                }
            }
        }
    }
}
