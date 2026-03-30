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
        private bool generationSettingsFoldout = true;

        private bool editorPreviewFoldout = true;
        private bool generatorDebugFoldout = true;

        private HexRoomGenerator mainGen;
        private HexRoomGeneratorPreview preview;

        private SerializedObject previewSerializedObject;

        private void OnEnable()
        {
            mainGen = (HexRoomGenerator)target;

            preview = mainGen.GetComponent<HexRoomGeneratorPreview>();

            previewSerializedObject = new SerializedObject(preview);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject
            previewSerializedObject.Update();

            DrawGenerationSettingsSection();
            DrawEditorPreviewSection();
            DrawGeneratorDebugSection();

            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
            previewSerializedObject.ApplyModifiedProperties();
        }

        #region Generation Settings

        private void DrawGenerationSettingsSection()
        {
            EditorHelper.DrawFoldout(ref generationSettingsFoldout, "Generation Settings", () =>
            {
                EditorHelper.DrawProperty("startAxial", serializedObject);

                EditorGUILayout.Space(6);

                var generationModeProp = serializedObject.FindProperty("generationMode");
                bool isGenerationRandomized = generationModeProp.enumValueIndex == (int)GenerationMode.Randomized;
                var generatorProp = isGenerationRandomized ? "randomizedGenerator" : "shapeGenerator";

                EditorHelper.DrawProperty("generationMode", serializedObject);
                EditorHelper.DrawProperty(generatorProp, serializedObject);

                //DrawButton("Randomize Seed", EditorRandomizeSeedInternal);
            });
        }

        #endregion Generation Settings


        #region Editor Preview
        private void DrawEditorPreviewSection()
        {
            EditorHelper.DrawFoldout(ref editorPreviewFoldout, "Editor Preview", () =>
            {
                var previewIsActiveProp = previewSerializedObject.FindProperty("previewIsActive");
                EditorGUILayout.PropertyField(previewIsActiveProp);

                if (previewIsActiveProp.boolValue)
                {
                    var colorProp = previewSerializedObject.FindProperty("previewHexColor");
                    var scaleProp = previewSerializedObject.FindProperty("previewHexScale");

                    EditorGUILayout.PropertyField(colorProp);
                    EditorGUILayout.PropertyField(scaleProp);

                    EditorGUILayout.BeginHorizontal();
                    EditorHelper.DrawButton("Rebuild Preview", preview.EditorForcePreviewRebuild);
                    EditorHelper.DrawButton("Clear Preview", preview.EditorClearPreviewInternal);
                    EditorGUILayout.EndHorizontal();
                }
            });
        }
        #endregion Editor Preview

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
