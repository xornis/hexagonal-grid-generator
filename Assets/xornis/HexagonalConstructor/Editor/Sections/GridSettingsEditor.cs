#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    [CustomEditor(typeof(HexGridSettings))]
    public class GridSettingsEditor : UnityEditor.Editor, IEditorSection
    {
        private HexGridSettings gridSettings;
        private bool foldout = true;

        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        private void OnEnable()
        {
            gridSettings = (HexGridSettings)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            EditorHelper.DrawFoldout(ref foldout, gridSettings.GetType().Name, () =>
            {
                DrawTileVisuals();
                DrawTileGeometry();
            });
        }

        private void DrawTileVisuals()
        {
            EditorHelper.DrawFoldout(ref tileVisualsFoldout, "Tile Visuals", () =>
            {
                EditorHelper.DrawProperties(serializedObject, "hexPrefab", "hexScale");
            });
        }

        private void DrawTileGeometry()
        {
            EditorHelper.DrawFoldout(ref tileGeometryFoldout, "Tile Geometry", () => 
            {
                EditorHelper.DrawProperties(serializedObject, "hexOrientation", "hexRadius");
            });
        }
    }
}
#endif
