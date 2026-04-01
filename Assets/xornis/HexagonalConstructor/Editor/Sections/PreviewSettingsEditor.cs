#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(HexPreviewSettings))]
    public class PreviewSettingsEditor : UnityEditor.Editor, IEditorSection
    {
        private HexPreviewSettings previewSettings;
        private bool foldout = true;

        private void OnEnable()
        {
            previewSettings = (HexPreviewSettings)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            EditorHelper.DrawFoldout(ref foldout, previewSettings.GetType().Name, () =>
            {
                var previewIsActiveProp = serializedObject.FindProperty("previewIsActive");
                EditorHelper.DrawProperties(serializedObject, previewIsActiveProp.propertyPath);

                if (previewIsActiveProp.boolValue)
                {
                    DrawPreviewFields();
                    DrawPreviewButtons();
                }
            });
        }

        private void DrawPreviewFields()
        {
            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperties(serializedObject, "previewHexColor", "previewHexScale");
            });
        }

        private void DrawPreviewButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Preview", previewSettings.EditorForcePreviewRebuild);
            EditorHelper.DrawButton("Clear Preview", previewSettings.EditorClearPreviewInternal);
            EditorGUILayout.EndHorizontal();
        }
    }
}

#endif