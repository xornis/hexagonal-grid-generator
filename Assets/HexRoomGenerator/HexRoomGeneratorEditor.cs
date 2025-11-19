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

        if (GUILayout.Button("Generate")) gen.EditorGenerate();
        if (GUILayout.Button("Clear")) gen.EditorClear();
        if (GUILayout.Button("Randomize Seed")) gen.EditorRandomizeSeed();
        if (GUILayout.Button("Randomize Seed and Generate")) gen.EditorRandomizeSeedAndGenerate();
    }
}
#endif
