#if UNITY_EDITOR

using UnityEditor;

namespace HexDungeon.Editor
{
    public class DebugSettingsSection
    {
        private readonly SerializedObject serializedObject;
        private readonly HexRoomGenerator mainGenerator;

        private bool foldout = true;

        public DebugSettingsSection(SerializedObject serializedObject, HexRoomGenerator generator)
        {
            this.serializedObject = serializedObject;
            mainGenerator = generator;
        }

        public void Draw()
        {
            foldout = EditorGUILayout.Foldout(foldout, "Debug Settings", true, EditorStyles.foldoutHeader);
            if (!foldout) return;

            var debugModeProp = serializedObject.FindProperty("debugMode");
            EditorHelper.DrawProperty("debugMode", serializedObject);

            if (debugModeProp.boolValue)
            {
                DrawFields();
                DrawButtons();
            }
        }

        private void DrawFields()
        {
            EditorHelper.DrawProperty("stepDelay", serializedObject);
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Generation", mainGenerator.EditorGenerateInternal);
            EditorHelper.DrawButton("Clear Generation", mainGenerator.EditorClearInternal);
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
