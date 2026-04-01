#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    public class GridSettingsEditor : UnityEditor.Editor
    {
        private HexGridSettings gridSettings;

        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        private void OnEnable()
        {
            gridSettings = (HexGridSettings)target;
        }

        public override void OnInspectorGUI()
        {
            Draw();
        }

        public void Draw()
        {
            serializedObject.Update();

            EditorHelper.Indent(() =>
            {
                DrawTileVisuals();
                DrawTileGeometry();
            });
            
            serializedObject.ApplyModifiedProperties();
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
