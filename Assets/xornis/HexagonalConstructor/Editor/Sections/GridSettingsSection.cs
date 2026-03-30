#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    public class GridSettingsSection
    {
        private readonly SerializedObject serializedObject;

        private bool foldout = true;
        private bool tileVisualsFoldout = true;
        private bool tileGeometryFoldout = true;

        public GridSettingsSection(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }

        public void Draw()
        {
            foldout = EditorGUILayout.Foldout(foldout, "Grid Settings", true, EditorStyles.foldoutHeader);
            if (!foldout) return;

            EditorHelper.Indent(() =>
            {
                DrawTileVisuals();
                DrawTileGeometry();
            });

            EditorGUILayout.Space(4);
        }

        private void DrawTileVisuals()
        {
            tileVisualsFoldout = EditorGUILayout.Foldout(tileVisualsFoldout, "Tile Visuals", true, EditorStyles.foldoutHeader);
            if (!tileVisualsFoldout) return;

            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperty("hexPrefab", serializedObject);
                EditorHelper.DrawProperty("hexScale", serializedObject);
            });
        }

        private void DrawTileGeometry()
        {
            tileGeometryFoldout = EditorGUILayout.Foldout(tileGeometryFoldout, "Tile Geometry", true, EditorStyles.foldoutHeader);
            if (!tileGeometryFoldout) return;

            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperty("hexOrientation", serializedObject);
                EditorHelper.DrawProperty("hexRadius", serializedObject);
            });
        }
    }
}
#endif
