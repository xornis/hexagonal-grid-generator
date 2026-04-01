#if UNITY_EDITOR

using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(DebugSettings))]
    public class DebugSettingsEditor : UnityEditor.Editor, IEditorSection
    {
        private DebugSettings debugSettings;
        private bool foldout = true;

        private void OnEnable()
        {
            debugSettings = (DebugSettings)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            EditorHelper.DrawFoldout(ref foldout, debugSettings.GetType().Name, () =>
            {
                var debugModeProp = serializedObject.FindProperty("isActive");
                EditorHelper.DrawProperties(serializedObject, debugModeProp.propertyPath);

                if (debugModeProp.boolValue)
                {
                    DrawFields();
                    DrawButtons();
                }
            });
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
            EditorHelper.DrawButton("Rebuild Generation", debugSettings.EditorGenerate);
            EditorHelper.DrawButton("Clear Generation", debugSettings.EditorClear);
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
