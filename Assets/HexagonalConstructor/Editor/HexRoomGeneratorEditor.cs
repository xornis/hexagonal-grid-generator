#if UNITY_EDITOR
using HexDungeon;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexRoomGenerator))]
public class HexRoomGeneratorEditor : Editor
{
    private bool generalFoldout = true;
    private bool visualFoldout = true;
    private bool geometryFoldout = true;
    private bool generationFoldout = true;
    private bool randomFoldout = true;
    private bool shapesFoldout = true;
    private bool previewFoldout = true;
    private bool debugFoldout = true;
    private HexRoomGenerator gen;

    private void OnEnable() => gen = (HexRoomGenerator)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject

        DrawSection("General", ref generalFoldout, "orientation");
        DrawSection("Visual", ref visualFoldout, "hexPrefab", "hexScale");
        DrawSection("Geometry", ref geometryFoldout, "hexSize");
        DrawGenerationSection();
        DrawPreviewSection();
        DrawDebugSection();

        serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector

        EditorGUILayout.Space(20);
    }

    private void DrawGenerationSection()
    {
        generationFoldout = EditorGUILayout.Foldout(generationFoldout, "Generation", true, EditorStyles.foldoutHeader);
        if (!generationFoldout) return;

        Indent(() =>
        {
            var modeProp = serializedObject.FindProperty("mode");
            EditorGUILayout.PropertyField(modeProp);

            bool isRandom = modeProp.enumValueIndex == (int)GenerationMode.Randomized;
            bool isShapes = modeProp.enumValueIndex == (int)GenerationMode.Shapes;

            GUIToggle(isRandom, () =>
            DrawRandomizedSection(isRandom)); // Section is visible only when mode is Randomized

            GUIToggle(isShapes, () =>
            DrawShapesSection(isShapes)); // Section is visible only when mode is Randomized
        });
    }

    private void DrawPreviewSection()
    {
        previewFoldout = EditorGUILayout.Foldout(previewFoldout, "Preview", true, EditorStyles.foldoutHeader);
        if (!previewFoldout) return;

        Indent(() =>
        {
            var previewInEditorProp = serializedObject.FindProperty("previewInEditor");
            EditorGUILayout.PropertyField(previewInEditorProp);
            bool isPreviewInEditor = previewInEditorProp.boolValue;

            GUIToggle(isPreviewInEditor, () =>
            {
                DrawProp("gizmoColor");
                DrawProp("gizmoHexScale");

                EditorGUILayout.BeginHorizontal();
                DrawButton("Build/Rebuild Preview", () => gen.EditorForcePreviewRebuild());
                DrawButton("Clear Preview", () => gen.EditorClearPreviewInternal());
                EditorGUILayout.EndHorizontal();
            });
        });
    }

    private void DrawDebugSection()
    {
        debugFoldout = EditorGUILayout.Foldout(debugFoldout, "Debug", true, EditorStyles.foldoutHeader);
        if (!debugFoldout) return;

        Indent(() =>
        {
            var debugModeProp = serializedObject.FindProperty("debugMode");
            EditorGUILayout.PropertyField(debugModeProp);
            bool isDebugMode = debugModeProp.boolValue;

            GUIToggle(isDebugMode, () =>
            {
                DrawProp("hexGenerationDelay");

                EditorGUILayout.BeginHorizontal();
                DrawButton("Build/Rebuild Generation", gen.EditorGenerateInternal);
                DrawButton("Clear Generation", gen.EditorClearInternal);
                EditorGUILayout.EndHorizontal();
            });
        });
    }

    private void DrawRandomizedSection(bool isRandom)
    {
        randomFoldout = EditorGUILayout.Foldout(randomFoldout, "Randomized", true, EditorStyles.foldoutHeader);
        if (!randomFoldout) return;

        Indent(() =>
        {
            DrawProp("randomType");
            DrawProp("rooms");

            var useSeedProp = serializedObject.FindProperty("useSeed");
            EditorGUILayout.PropertyField(useSeedProp);
            bool isUsingSeed = useSeedProp.boolValue;

            GUIToggle(isUsingSeed && isRandom, () =>
            {
                EditorGUILayout.BeginHorizontal();

                DrawProp("seed");
                DrawButton("Randomize", () => 
                { 
                    gen.EditorRandomizeSeedInternal(); 
                    serializedObject.ApplyModifiedProperties();
                });

                EditorGUILayout.EndHorizontal();
            });
        });

        EditorGUILayout.Space(10);
    }

    private void DrawShapesSection(bool isShaped)
    {
        shapesFoldout = EditorGUILayout.Foldout(shapesFoldout, "Shapes", true, EditorStyles.foldoutHeader);
        if (!shapesFoldout) return;

        Indent(() =>
        {
            var shapeTypeProp = serializedObject.FindProperty("shapeType");
            EditorGUILayout.PropertyField(shapeTypeProp);

            bool useSpiralShape = shapeTypeProp.enumValueIndex == (int)HexShapeType.Spiral;

            DrawIf(!useSpiralShape, "radius");
            DrawIf(useSpiralShape, "hexCount", "growth", "startDirection");
        });
    }

    private void DrawSection(string title, ref bool foldout, params string[] props)
    {
        foldout = EditorGUILayout.Foldout(foldout, title, true, EditorStyles.foldoutHeader);
        if (!foldout) return;

        Indent(() =>
        {
            foreach (var propName in props)
                DrawProp(propName);
        });

        EditorGUILayout.Space(10);
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

    private void GUIToggle(bool enabled, Action body)
    {
        bool prev = GUI.enabled;
        GUI.enabled = enabled;
        body?.Invoke();
        GUI.enabled = prev;
    }

    private void DrawIf(bool condition, params string[] names)
    {
        if (!condition) return;
        foreach (var p in names) DrawProp(p);
    }

}
#endif
