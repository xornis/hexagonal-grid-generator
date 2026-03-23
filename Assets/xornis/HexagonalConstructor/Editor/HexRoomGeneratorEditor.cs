#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace HexDungeon
{
    [CustomEditor(typeof(HexRoomGenerator))]
    public class HexRoomGeneratorEditor : Editor
    {
        private bool gridSettingsFoldout = true;
        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        private bool generationSettingsFoldout = true;
        private bool randomGenerationFoldout = true;
        private bool shapeGenerationFoldout = true;

        private bool editorPreviewFoldout = true;
        private bool generatorDebugFoldout = true;

        private HexRoomGenerator gen;

        private void OnEnable() => gen = (HexRoomGenerator)target;

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject

            DrawGridSettingsSection();
            DrawGenerationSettingsSection();
            DrawEditorPreviewSection();
            DrawGeneratorDebugSection();

            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
        }

        #region Grid Settings

        private void DrawGridSettingsSection()
        {
            DrawFoldout(ref gridSettingsFoldout, "Grid Settings", () =>
            {
                DrawTileVisualsSection();
                DrawTileGeometrySection();
            });
        }

        #region Tile Visuals
        private void DrawTileVisualsSection()
        {
            DrawFoldout(ref tileVisualsFoldout, "Tile Visuals", () =>
            {
                DrawProp("hexPrefab");
                DrawProp("hexScale");
            });
        }
        #endregion Tile Visuals


        #region Tile Geometry
        private void DrawTileGeometrySection()
        {
            DrawFoldout(ref tileGeometryFoldout, "Tile Geometry", () =>
            {
                DrawProp("hexOrientation");
                DrawProp("hexRadius");
            });
        }
        #endregion Tile Geometry

        #endregion Grid Settings


        #region Generation Settings

        private void DrawGenerationSettingsSection()
        {
            DrawFoldout(ref generationSettingsFoldout, "Generation Settings", () =>
            {
                DrawProp("startAxial");

                EditorGUILayout.Space(6);

                var generationModeProp = serializedObject.FindProperty("generationMode");
                var generatorProp = generationModeProp.enumValueIndex == (int)GenerationMode.Shapes ? serializedObject.FindProperty("shapeGenerator") : serializedObject.FindProperty("randomizedGenerator");

                EditorGUILayout.PropertyField(generationModeProp, true);
                EditorGUILayout.PropertyField(generatorProp, true);
            });
        }

        #endregion Generation Settings


        #region Editor Preview
        private void DrawEditorPreviewSection()
        {
            DrawFoldout(ref editorPreviewFoldout, "Editor Preview", () =>
            {
                var enablePreviewProp = serializedObject.FindProperty("enablePreview");
                EditorGUILayout.PropertyField(enablePreviewProp);
                bool isEnablePreview = enablePreviewProp.boolValue;

                if (isEnablePreview)
                {
                    DrawProp("previewColor");
                    DrawProp("previewHexScale");

                    EditorGUILayout.BeginHorizontal();
                    DrawButton("Rebuild Preview", gen.EditorForcePreviewRebuild);
                    DrawButton("Clear Preview", gen.EditorClearPreviewInternal);
                    EditorGUILayout.EndHorizontal();
                }
            });
        }
        #endregion Editor Preview

        #region Generator Debug
        private void DrawGeneratorDebugSection()
        {
            DrawFoldout(ref generatorDebugFoldout, "Generator Debug", () =>
            {
                var debugModeProp = serializedObject.FindProperty("debugMode");
                EditorGUILayout.PropertyField(debugModeProp);
                bool isDebugMode = debugModeProp.boolValue;

                if (isDebugMode)
                {
                    DrawProp("stepDelay");

                    EditorGUILayout.BeginHorizontal();
                    DrawButton("Rebuild Generation", gen.EditorGenerateInternal);
                    DrawButton("Clear Generation", gen.EditorClearInternal);
                    EditorGUILayout.EndHorizontal();
                }
            });
        }
        #endregion Generator Debug

        #region Editor Helpers

        private void DrawProp(string name)
        {
            var prop = serializedObject.FindProperty(name);
            if (prop != null) EditorGUILayout.PropertyField(prop, true);
            else EditorGUILayout.HelpBox($"Property '{name}' not found", MessageType.Warning);
        }

        private void DrawButton(string name, Action onClick)
        {
            if (GUILayout.Button(name))
                onClick?.Invoke();
        }

        private void Indent(Action body)
        {
            EditorGUI.indentLevel++;
            body();
            EditorGUI.indentLevel--;
        }

        private void DrawIf(bool condition, params string[] names)
        {
            if (!condition) return;
            foreach (var p in names) DrawProp(p);
        }

        private void DrawFoldout(ref bool state, string title, Action body)
        {
            state = EditorGUILayout.Foldout(state, title, true, EditorStyles.foldoutHeader);
            if (!state) return;

            Indent(body);
            EditorGUILayout.Space(4);
        }

        #endregion Editor Helpers
    }
}
#endif
