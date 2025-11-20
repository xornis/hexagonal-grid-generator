#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexDungeon.HexRoomGenerator))]
public class HexRoomGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexDungeon.HexRoomGenerator gen = (HexDungeon.HexRoomGenerator)target;

        GUILayout.Space(20);
        GUILayout.Label("Editor Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate")) gen.EditorGenerate();
        if (GUILayout.Button("Clear")) gen.EditorClear();

        GUILayout.Space(10);

        if (GUILayout.Button("Show Gizmos Preview")) gen.EditorShowGizmosPreview();
        if (GUILayout.Button("Clear Gizmos Preview")) gen.EditorClearPreview();

        GUILayout.Space(10);

        if (GUILayout.Button("Randomize Seed")) gen.EditorRandomizeSeed();
    }
}
#endif
