#if UNITY_EDITOR
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HexDungeon.Editor
{
    public static class EditorHelper 
    {
        public static void DrawProperty(string name, SerializedObject serializedObject)
        {
            var prop = serializedObject.FindProperty(name);
            if (prop != null) EditorGUILayout.PropertyField(prop, true);
            else EditorGUILayout.HelpBox($"Property '{name}' not found", MessageType.Warning);
        }

        public static void DrawButton(string name, Action onClick)
        {
            if (GUILayout.Button(name))
                onClick?.Invoke();
        }

        public static void Indent(Action body)
        {
            EditorGUI.indentLevel++;
            body();
            EditorGUI.indentLevel--;
        }

        public static void DrawFoldout(ref bool state, string title, Action body)
        {
            state = EditorGUILayout.Foldout(state, Regex.Replace(title, "([a-z])([A-Z])", "$1 $2"), true, EditorStyles.foldoutHeader);
            if (!state) return;

            Indent(body);
            EditorGUILayout.Space(4);
        }
    }
}
#endif