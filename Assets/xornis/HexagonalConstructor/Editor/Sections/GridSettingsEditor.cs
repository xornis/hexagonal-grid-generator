#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    [CustomEditor(typeof(HexGridSettings))]
    public class GridSettingsEditor : UnityEditor.Editor
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
                EditorHelper.DrawProperty("hexPrefab", serializedObject);
                EditorHelper.DrawProperty("hexScale", serializedObject);
            });
        }

        private void DrawTileGeometry()
        {
            EditorHelper.DrawFoldout(ref tileGeometryFoldout, "Tile Geometry", () => 
            {
                EditorHelper.DrawProperty("hexOrientation", serializedObject);
                EditorHelper.DrawProperty("hexRadius", serializedObject);
            });
        }
    }
}
#endif
