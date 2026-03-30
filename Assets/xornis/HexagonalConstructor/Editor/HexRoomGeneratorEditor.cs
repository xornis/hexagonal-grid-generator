#if UNITY_EDITOR
using HexDungeon.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace HexDungeon
{
    [CustomEditor(typeof(HexRoomGenerator))]
    public class HexRoomGeneratorEditor : UnityEditor.Editor
    {
        private bool generatorDebugFoldout = true;

        private HexRoomGenerator mainGen;

        private void OnEnable()
        {
            mainGen = (HexRoomGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject

            DrawGeneratorDebugSection();

            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
        }

        #region Generator Debug
        private void DrawGeneratorDebugSection()
        {
            EditorHelper.DrawFoldout(ref generatorDebugFoldout, "Generator Debug", () =>
            {
                var debugModeProp = serializedObject.FindProperty("debugMode");
                EditorGUILayout.PropertyField(debugModeProp);
                bool isDebugMode = debugModeProp.boolValue;

                if (isDebugMode)
                {
                    EditorHelper.DrawProperty("stepDelay", serializedObject);

                    EditorGUILayout.BeginHorizontal();
                    EditorHelper.DrawButton("Rebuild Generation", mainGen.EditorGenerateInternal);
                    EditorHelper.DrawButton("Clear Generation", mainGen.EditorClearInternal);
                    EditorGUILayout.EndHorizontal();
                }
            });
        }
        #endregion Generator Debug
    }
}
#endif
