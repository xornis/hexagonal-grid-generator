#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(GridSettings))]
    public class GridSettingsEditor : UnityEditor.Editor, IEditorSection
    {
        private GridSettings gridSettings;
        private bool foldout = true;

        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        private void OnEnable()
        {
            gridSettings = (GridSettings)target;
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
