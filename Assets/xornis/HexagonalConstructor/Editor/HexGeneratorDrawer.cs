#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexagonalConstructor
{
    [CustomPropertyDrawer(typeof(SerializableGridGenerator), true)]
    public class HexGeneratorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var buttonRect = GetAndShowPropertyDropdown(position, property, label);

            EditorGUI.indentLevel++; ShowChildFields(position, property, buttonRect);
            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        private void ShowTypeMenu(SerializedProperty property, Rect buttonRect)
        {
            var menu = new GenericMenu();
            foreach (var type in GetGeneratorTypes(property))
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });

            menu.DropDown(buttonRect);
        }

        private Rect GetAndShowPropertyDropdown(Rect position, SerializedProperty property, GUIContent label)
        {
            var buttonRect = EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);
            var typeName = property.managedReferenceValue?.GetType().Name ?? "None";

            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(typeName), FocusType.Passive))
                ShowTypeMenu(property, buttonRect);

            return buttonRect;
        }

        private void ShowChildFields(Rect position, SerializedProperty property, Rect buttonRect)
        {
            var child = property.Copy();
            var endProperty = property.GetEndProperty();
            child.NextVisible(true);

            float y = buttonRect.yMax + EditorGUIUtility.standardVerticalSpacing;

            while (!SerializedProperty.EqualContents(child, endProperty))
            {
                float height = EditorGUI.GetPropertyHeight(child, true);
                var fieldRect = new Rect(position.x, y, position.width, height);
                EditorGUI.PropertyField(fieldRect, child, true);
                y += height;
                y += EditorGUIUtility.standardVerticalSpacing;
                child.NextVisible(false);
            }
        }

        private Type[] GetGeneratorTypes(SerializedProperty property)
        {
            bool isShape = typeof(ShapeGenerator).IsAssignableFrom(fieldInfo.FieldType);

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => (isShape 
                    ? typeof(ShapeGenerator) 
                    : typeof(RandomizedGenerator))
                    .IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            foreach (SerializedProperty child in property)
                height += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
    }
}
#endif