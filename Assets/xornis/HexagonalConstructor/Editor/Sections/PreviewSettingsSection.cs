#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    public class PreviewSettingsSection
    {
        private readonly SerializedObject serializedObject;
        private readonly HexRoomGeneratorPreview previewGenerator;
        private readonly HexRoomGenerator mainGenerator;

        private bool foldout = true;

        public PreviewSettingsSection(SerializedObject serializedObject, HexRoomGenerator generator)
        {
            this.serializedObject = serializedObject;

            mainGenerator = generator;
            previewGenerator = generator.GetComponent<HexRoomGeneratorPreview>();
            if (previewGenerator != null)
                serializedObject = new SerializedObject(previewGenerator);
        }

        public void Draw()
        {
            foldout = EditorGUILayout.Foldout(foldout, "Preview Settings", true, EditorStyles.foldoutHeader);
            if (!foldout) return;

            EditorHelper.Indent(() =>
            {
                var previewIsActiveProp = serializedObject.FindProperty("previewIsActive");
                EditorHelper.DrawProperty("previewIsActive", serializedObject);

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
                EditorHelper.DrawProperty("previewHexColor", serializedObject);
                EditorHelper.DrawProperty("previewHexScale", serializedObject);
            });
        }

        private void DrawPreviewButtons()
        {
            EditorGUILayout.BeginHorizontal();
            EditorHelper.DrawButton("Rebuild Preview", previewGenerator.EditorForcePreviewRebuild);
            EditorHelper.DrawButton("Clear Preview", previewGenerator.EditorClearPreviewInternal);
            EditorGUILayout.EndHorizontal();
        }
    }
}

#endif