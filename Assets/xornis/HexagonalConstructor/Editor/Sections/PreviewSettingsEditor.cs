#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(PreviewSettings))]
    public class PreviewSettingsEditor : SettingsEditorBase
    {
        private bool foldout = true;

        private PreviewSettings Settings => (PreviewSettings)target;

        public override void Draw()
        {
            serializedObject.Update();

            EditorHelper.DrawFoldout(ref foldout, Settings.GetType().Name, () =>
            {
                var previewIsActiveProp = serializedObject.FindProperty("isActive");
                EditorHelper.DrawProperties(serializedObject, previewIsActiveProp.propertyPath);

                if (previewIsActiveProp.boolValue)
                {
                    DrawPreviewFields();
                    DrawPreviewButtons();
                }
            });

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPreviewFields()
        {
            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperties(serializedObject, "hexColor", "hexScale");
            });
        }

        private void DrawPreviewButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Preview", Settings.EditorForcePreviewRebuild);
            EditorHelper.DrawButton("Clear Preview", Settings.EditorClearPreviewInternal);
            EditorGUILayout.EndHorizontal();
        }
    }
}

#endif