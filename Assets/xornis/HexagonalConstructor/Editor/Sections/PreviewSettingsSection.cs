#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    public class PreviewSettingsSection
    {
        private SerializedObject previewSerializedObject;
        private HexPreviewSettings previewGenerator;
        private readonly HexRoomGenerator mainGenerator;

        private bool foldout = true;

        public PreviewSettingsSection(HexRoomGenerator generator)
        {
            mainGenerator = generator;
            RefreshPreviewReference();
        }

        public void Draw()
        {
            if (previewGenerator == null)
            {
                previewGenerator = mainGenerator.gameObject.AddComponent<HexPreviewSettings>();
                RefreshPreviewReference();
                return;
            }

            previewSerializedObject.Update();

            EditorHelper.DrawFoldout(ref foldout, "Preview Settings", () =>
            {
                EditorHelper.Indent(() =>
                {
                    var previewIsActiveProp = previewSerializedObject.FindProperty("previewIsActive");
                    EditorHelper.DrawProperty("previewIsActive", previewSerializedObject);

                    if (previewIsActiveProp.boolValue)
                    {
                        DrawPreviewFields();
                        DrawPreviewButtons();
                    }
                });
            });
            
            previewSerializedObject.ApplyModifiedProperties();
        }

        private void DrawPreviewFields()
        {
            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperty("previewHexColor", previewSerializedObject);
                EditorHelper.DrawProperty("previewHexScale", previewSerializedObject);
            });
        }

        private void DrawPreviewButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Preview", previewGenerator.EditorForcePreviewRebuild);
            EditorHelper.DrawButton("Clear Preview", previewGenerator.EditorClearPreviewInternal);
            EditorGUILayout.EndHorizontal();
        }

        private void RefreshPreviewReference()
        {
            previewGenerator = mainGenerator.GetComponent<HexPreviewSettings>();

            if (previewGenerator != null)
                previewSerializedObject = new SerializedObject(previewGenerator);
        }
    }
}

#endif