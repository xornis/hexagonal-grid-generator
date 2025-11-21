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

        EditorGUI.indentLevel++;

        var modeProp = serializedObject.FindProperty("mode");
        EditorGUILayout.PropertyField(modeProp);

        bool isRandom = modeProp.enumValueIndex == (int)GenerationMode.Randomized;
        bool isShapes = modeProp.enumValueIndex == (int)GenerationMode.Shapes;

        GUI.enabled = isRandom; // Section is visible only when mode is Randomized
        DrawRandomSection();
        GUI.enabled = true;

        GUI.enabled = isShapes; // Section is visible only when mode is Shapes
        DrawSection("Shapes", ref shapesFoldout, "shapeType", "radius", "corridorThickness");
        GUI.enabled = true;

        EditorGUI.indentLevel--;
    }

    private void DrawPreviewSection()
    {
        previewFoldout = EditorGUILayout.Foldout(previewFoldout, "Preview", true, EditorStyles.foldoutHeader);
        if (!previewFoldout) return;

        EditorGUI.indentLevel++;

        var previewInEditorProp = serializedObject.FindProperty("previewInEditor");
        EditorGUILayout.PropertyField(previewInEditorProp);
        bool isPreviewInEditor = previewInEditorProp.boolValue;

        GUI.enabled = isPreviewInEditor;

        DrawProp("gizmoColor");
        DrawProp("gizmoHexScale");

        EditorGUILayout.BeginHorizontal();
        DrawButton("Build/Rebuild Preview", () => gen.EditorForcePreviewRebuild());
        DrawButton("Clear Preview", () => gen.EditorClearPreviewInternal());
        EditorGUILayout.EndHorizontal();

        GUI.enabled = true;

        EditorGUI.indentLevel--;
    }

    private void DrawDebugSection()
    {
        debugFoldout = EditorGUILayout.Foldout(debugFoldout, "Debug", true, EditorStyles.foldoutHeader);
        if (!debugFoldout) return;

        EditorGUI.indentLevel++;

        var debugModeProp = serializedObject.FindProperty("debugMode");
        EditorGUILayout.PropertyField(debugModeProp);
        bool isDebugMode = debugModeProp.boolValue;

        GUI.enabled = isDebugMode;

        DrawProp("debugStepDelay");

        EditorGUILayout.BeginHorizontal();
        DrawButton("Build/Rebuild Generation", gen.EditorGenerateInternal);
        DrawButton("Clear Generation", gen.EditorClearInternal);
        EditorGUILayout.EndHorizontal();

        GUI.enabled = true;

        EditorGUI.indentLevel--;
    }

    private void DrawRandomSection()
    {
        randomFoldout = EditorGUILayout.Foldout(randomFoldout, "Random", true, EditorStyles.foldoutHeader);
        if (!randomFoldout) return;

        EditorGUI.indentLevel++;

        DrawProp("randomType");
        DrawProp("rooms");

        var useSeedProp = serializedObject.FindProperty("useSeed");
        EditorGUILayout.PropertyField(useSeedProp);
        bool isUsingSeed = useSeedProp.boolValue;

        GUI.enabled = isUsingSeed;
        EditorGUILayout.BeginHorizontal();

        DrawProp("seed");

        DrawButton("Randomize", gen.EditorRandomizeSeedInternal);

        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        EditorGUI.indentLevel--;
        EditorGUILayout.Space(10);
    }

    private void DrawSection(string title, ref bool foldout, params string[] props)
    {
        foldout = EditorGUILayout.Foldout(foldout, title, true, EditorStyles.foldoutHeader);
        if (!foldout) return;

        EditorGUI.indentLevel++;

        foreach (var propName in props)
            DrawProp(propName);

        EditorGUI.indentLevel--;

        EditorGUILayout.Space(10);
    }

    private void DrawProp(string name)
    {
        var prop = serializedObject.FindProperty(name);
        if (prop != null) EditorGUILayout.PropertyField(prop);
        else EditorGUILayout.HelpBox($"Property '{prop}' not found", MessageType.Warning);
    }

    private void DrawButton(string name, Action onClick)
    {
        if (GUILayout.Button(name))
            onClick?.Invoke();
    }
}
#endif
