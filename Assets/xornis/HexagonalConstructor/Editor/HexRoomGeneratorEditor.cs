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
                var modeProp = serializedObject.FindProperty("mode");
                EditorGUILayout.PropertyField(modeProp);
                DrawProp("startAxial");

                bool isRandomized = modeProp.enumValueIndex == (int)GenerationMode.Randomized;
                bool isShapes = modeProp.enumValueIndex == (int)GenerationMode.Shapes;

                EditorGUILayout.Space(6);

                if (isRandomized) DrawRandomGenerationSection(isRandomized); // Section is visible only when mode is Randomized
                if (isShapes) DrawShapeGenerationSection(); // Section is visible only when mode is Randomized
            });
        }

        #region Random Generation
        private void DrawRandomGenerationSection(bool isRandom)
        {
            DrawFoldout(ref randomGenerationFoldout, "Random Generation", () =>
            {
                DrawProp("randomAlgorithm");
                DrawProp("roomCount");

                var useSeedProp = serializedObject.FindProperty("useSeed");
                EditorGUILayout.PropertyField(useSeedProp);
                bool isUsingSeed = useSeedProp.boolValue;

                if (isUsingSeed && isRandom)
                {
                    EditorGUILayout.BeginHorizontal();

                    DrawProp("seed");
                    DrawButton("Randomize", () =>
                    {
                        gen.EditorRandomizeSeedInternal();
                        serializedObject.ApplyModifiedProperties();
                    });

                    EditorGUILayout.EndHorizontal();
                }
            });
        }
        #endregion Random Generation

        #region Shape Generation
        private void DrawShapeGenerationSection()
        {
            DrawFoldout(ref shapeGenerationFoldout, "Shape Generation", () =>
            {
                var shapeTypeProp = serializedObject.FindProperty("shape");
                EditorGUILayout.PropertyField(shapeTypeProp);

                bool useSpiralShape = shapeTypeProp.enumValueIndex == (int)HexShape.Spiral;
                bool useTriangleShape = shapeTypeProp.enumValueIndex == (int)HexShape.Triangle;

                DrawIf(useSpiralShape, "spiralLength", "growthAmount", "startDirection");
                DrawIf(useTriangleShape, "triangleSideLength");
                DrawIf(!useSpiralShape && !useTriangleShape, "shapeRadius");
            });
        }
        #endregion Shape Generation

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
            if (prop != null) EditorGUILayout.PropertyField(prop);
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
