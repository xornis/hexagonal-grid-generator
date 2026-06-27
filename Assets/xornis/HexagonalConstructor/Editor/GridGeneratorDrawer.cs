#if UNITY_EDITOR

using HexagonalConstructor.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexagonalConstructor
{
    [CustomPropertyDrawer(typeof(SerializableGridGenerator), true)]
    public class GridGeneratorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            var buttonRect = GetAndShowPropertyDropdown(position, property, label);

            EditorGUI.indentLevel++; 
            ShowChildFields(position, property, buttonRect);
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                PreviewSettings.InvokeForceRebuild();
            }

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
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                    PreviewSettings.InvokeForceRebuild();
                });

            menu.DropDown(buttonRect);
        }

        private Rect GetAndShowPropertyDropdown(Rect position, SerializedProperty property, GUIContent label)
        {
            var buttonRect = EditorGUI.PrefixLabel(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);
            var typeName = property.managedReferenceValue?.GetType().Name ?? "Choose Generator Type";

            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(typeName), FocusType.Passive))
                ShowTypeMenu(property, buttonRect);

            return buttonRect;
        }

        private void ShowChildFields(Rect position, SerializedProperty property, Rect buttonRect)
        {
            if (property.managedReferenceValue == null) return;

            var propertyChild = property.Copy();
            var nextElement = propertyChild.NextVisible(true);

            var useSeedProp = property.FindPropertyRelative("useSeed");
            
            float y = buttonRect.yMax + EditorGUIUtility.standardVerticalSpacing;

            int parentDepth = property.depth;

            while (nextElement && propertyChild.depth > parentDepth)
            {
                if (propertyChild.name == "seed" && useSeedProp != null && !useSeedProp.boolValue)
                {
                    nextElement = propertyChild.NextVisible(false);
                    continue;
                }

                float height = EditorGUI.GetPropertyHeight(propertyChild, true);
                var fieldRect = new Rect(position.x, y, position.width, height);

                EditorGUI.PropertyField(fieldRect, propertyChild, true);
                y += height + EditorGUIUtility.standardVerticalSpacing;

                nextElement = propertyChild.NextVisible(false);
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
            float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (property.managedReferenceValue == null) return totalHeight;

            var propertyChild = property.Copy();
            var nextElement = propertyChild.NextVisible(true);
            var useSeedProp = property.FindPropertyRelative("useSeed");

            int parentDepth = property.depth;

            while (nextElement && propertyChild.depth > parentDepth)
            {
                if (propertyChild.name == "seed" && useSeedProp != null && !useSeedProp.boolValue)
                {
                    nextElement = propertyChild.NextVisible(false);
                    continue;
                }

                totalHeight += EditorGUI.GetPropertyHeight(propertyChild, true) + EditorGUIUtility.standardVerticalSpacing;
                nextElement = propertyChild.NextVisible(false);
            }

            return totalHeight;
        }
    }
}
#endif