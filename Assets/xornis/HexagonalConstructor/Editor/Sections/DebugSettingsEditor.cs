#if UNITY_EDITOR

using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(DebugSettings))]
    public class DebugSettingsEditor : SettingsEditorBase
    {
        private bool foldout = true;

        private DebugSettings Settings => (DebugSettings)target;

        public override void Draw()
        {
            serializedObject.Update();

            EditorHelper.DrawFoldout(ref foldout, Settings.GetType().Name, () =>
            {
                var debugModeProp = serializedObject.FindProperty("isActive");
                EditorHelper.DrawProperties(serializedObject, debugModeProp.propertyPath);

                if (debugModeProp.boolValue)
                {
                    DrawFields();
                    DrawButtons();
                }
            });

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFields()
        {
            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperties(serializedObject, "stepDelay");
            });
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Generation", Settings.EditorGenerate);
            EditorHelper.DrawButton("Clear Generation", Settings.EditorClear);
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
