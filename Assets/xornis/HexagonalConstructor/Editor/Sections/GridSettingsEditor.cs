#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(GridSettings))]
    public class GridSettingsEditor : SettingsEditorBase
    {
        private bool foldout = true;
        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        private GridSettings Settings => (GridSettings)target;

        public override void Draw()
        {
            serializedObject.Update();

            EditorHelper.DrawFoldout(ref foldout, Settings.GetType().Name, () =>
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
