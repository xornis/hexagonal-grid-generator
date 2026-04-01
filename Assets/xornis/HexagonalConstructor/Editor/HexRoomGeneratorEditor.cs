#if UNITY_EDITOR
using HexDungeon.Editor;
using UnityEditor;

namespace HexDungeon
{
    [CustomEditor(typeof(HexRoomGenerator))]
    public class HexRoomGeneratorEditor : UnityEditor.Editor
    {
        private HexRoomGenerator mainGen;

        private GridSettingsSection gridSettingsSection;
        private GenerationSettingsSection generationSettingsSection;
        private PreviewSettingsSection previewSettingsSection;
        private DebugSettingsEditor debugSettingsSection;

        private void OnEnable()
        {
            mainGen = (HexRoomGenerator)target;

            gridSettingsSection = new GridSettingsSection(serializedObject);
            generationSettingsSection = new GenerationSettingsSection(serializedObject);
            previewSettingsSection = new PreviewSettingsSection(mainGen);
            debugSettingsSection = new DebugSettingsEditor();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject

            gridSettingsSection.Draw();
            generationSettingsSection.Draw();
            previewSettingsSection.Draw();
            debugSettingsSection.Draw();

            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
        }
    }
}
#endif
