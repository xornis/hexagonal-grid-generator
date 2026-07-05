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

            var useSeedProp = property.FindPropertyRelative("useSeed");
            bool wasUsingSeed = useSeedProp != null && useSeedProp.boolValue;

            EditorGUI.BeginChangeCheck();

            var buttonRect = GetAndShowPropertyDropdown(position, property, label);

            EditorGUI.indentLevel++; 
            ShowChildFields(position, property, buttonRect);
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
            {
                bool isUsingSeedNow = useSeedProp != null && useSeedProp.boolValue;
                if (!wasUsingSeed && isUsingSeedNow)
                {
                    var seedProp = property.FindPropertyRelative("seed");
                    if (seedProp != null)
                    {
                        seedProp.intValue = UnityEngine.Random.Range(1, 999999);
                    }
                }

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

                if (propertyChild.name == "seed")
                {
                    float buttonWidth = 80f;
                    var valueRect = new Rect(fieldRect.x, fieldRect.y, fieldRect.width - buttonWidth - 4f, fieldRect.height);
                    var diceRect = new Rect(fieldRect.xMax - buttonWidth, fieldRect.y, buttonWidth, fieldRect.height);

                    EditorGUI.PropertyField(valueRect, propertyChild, true);

                    if (GUI.Button(diceRect, "Randomize"))
                    {
                        propertyChild.intValue = UnityEngine.Random.Range(-int.MaxValue, int.MaxValue);

                        property.serializedObject.ApplyModifiedProperties();
                        PreviewSettings.InvokeForceRebuild();
                    }
                }
                else EditorGUI.PropertyField(fieldRect, propertyChild, true);

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