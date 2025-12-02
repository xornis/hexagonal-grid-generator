#if UNITY_EDITOR
using HexDungeon;
using System;
using UnityEditor;
using UnityEngine;

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

    private void DrawGenerationSettingsSection()
    {
        generationSettingsFoldout = EditorGUILayout.Foldout(generationSettingsFoldout, "Generation Settings", true, EditorStyles.foldoutHeader);
        if (!generationSettingsFoldout) return;

        Indent(() =>
        {
            var modeProp = serializedObject.FindProperty("mode");
            EditorGUILayout.PropertyField(modeProp);

            bool isRandomized = modeProp.enumValueIndex == (int)GenerationMode.Randomized;
            bool isShapes = modeProp.enumValueIndex == (int)GenerationMode.Shapes;

            EditorGUILayout.Space(10);
            
            if (isRandomized) DrawRandomGenerationSection(isRandomized); // Section is visible only when mode is Randomized

            EditorGUILayout.Space(10);
            
            if (isShapes) DrawShapeGenerationSection(); // Section is visible only when mode is Randomized

            EditorGUILayout.Space(10);
        });
    }

    private void DrawEditorPreviewSection()
    {
        editorPreviewFoldout = EditorGUILayout.Foldout(editorPreviewFoldout, "Editor Preview", true, EditorStyles.foldoutHeader);
        if (!editorPreviewFoldout) return;

        Indent(() =>
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
            EditorGUILayout.Space(10);
        });
    }

    private void DrawGeneratorDebugSection()
    {
        generatorDebugFoldout = EditorGUILayout.Foldout(generatorDebugFoldout, "Generator Debug", true, EditorStyles.foldoutHeader);
        if (!generatorDebugFoldout) return;

        Indent(() =>
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
            EditorGUILayout.Space(10);
        });
    }

    private void DrawRandomGenerationSection(bool isRandom)
    {
        randomGenerationFoldout = EditorGUILayout.Foldout(randomGenerationFoldout, "Random Generation", true, EditorStyles.foldoutHeader);
        if (!randomGenerationFoldout) return;

        Indent(() =>
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

    private void DrawShapeGenerationSection()
    {
        shapeGenerationFoldout = EditorGUILayout.Foldout(shapeGenerationFoldout, "Shape Generation", true, EditorStyles.foldoutHeader);
        if (!shapeGenerationFoldout) return;

        Indent(() =>
        {
            var shapeTypeProp = serializedObject.FindProperty("shape");
            EditorGUILayout.PropertyField(shapeTypeProp);

            bool useSpiralShape = shapeTypeProp.enumValueIndex == (int)HexShape.Spiral;

            DrawIf(!useSpiralShape, "shapeRadius");
            DrawIf(useSpiralShape, "spiralLength", "growthAmount", "startDirection");
        });
    }

    private void DrawGridSettingsSection()
    {
        gridSettingsFoldout = EditorGUILayout.Foldout(gridSettingsFoldout, "Grid Settings", true, EditorStyles.foldoutHeader);
        if (!gridSettingsFoldout) return;

        Indent(() =>
        {
            DrawTileVisualsSection();

            EditorGUILayout.Space(10);

            DrawTileGeometrySection();

            EditorGUILayout.Space(10);
        });
    }

    private void DrawTileVisualsSection()
    {
        tileVisualsFoldout = EditorGUILayout.Foldout(tileVisualsFoldout, "Tile Visuals", true, EditorStyles.foldoutHeader);
        if (!tileVisualsFoldout) return;

        Indent(() =>
        {
            DrawProp("hexPrefab");
            DrawProp("hexScale");
        });
    }

    private void DrawTileGeometrySection()
    {
        tileGeometryFoldout = EditorGUILayout.Foldout(tileGeometryFoldout, "Tile Geometry", true, EditorStyles.foldoutHeader);
        if (!tileGeometryFoldout) return;

        Indent(() =>
        {
            DrawProp("hexOrientation");
            DrawProp("hexRadius");
        });
    }

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
}
#endif
