#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexDungeon
{
    [ExecuteInEditMode]
    public class HexPreviewSettings : MonoBehaviour
    {
        [SerializeField] private bool previewIsActive = true;
        [SerializeField] private Color previewHexColor = Color.blue;
        [SerializeField, Range(0.1f, 1.5f)] private float previewHexScale = 0.9f;

        private HexRoomContext context;

        private List<HexCoord> previewCache = new List<HexCoord>();
        private bool previewDirty = true;

        public void OnEnable()
        {
            context = GetComponent<HexRoomContext>();
        }

        private void RebuildPreview()
        {
            var previewGen = context.Generation.CurrentGenerator;

            if (previewGen == null)
            {
                previewDirty = false;
                return;
            }

            previewCache.Clear();

            foreach (var hex in previewGen.Generate(context.Generation.StartHex))
                previewCache.Add(hex);
        }

        private void OnDrawGizmosSelected()
        {
            if (!previewIsActive) return;
            if (!enabled) return;

            if (previewDirty)
                RebuildPreview();

            Handles.color = previewHexColor;

            foreach (var hex in previewCache)
            {
                Vector3 center = transform.TransformPoint(context.Grid.HexLayout.HexToWorld(hex));
                DrawHexHandle(center, context.Grid.HexLayout, previewHexScale);
            }
        }

        private void DrawHexHandle(Vector3 center, HexLayout layout, float scale)
        {
            float radius = layout.Size * scale;

            float startAngle = context.Grid.HexOrientation == HexOrientation.FlatTop ? 0f : 30f;

            Vector3 firstPoint = Vector3.zero;

            Vector3 prev = Vector3.zero;

            for (int i = 0; i <= 6; i++)
            {
                float angleDeg = startAngle + i * 60f;
                float angleRad = angleDeg * Mathf.Deg2Rad;

                Vector3 point = center + new Vector3(
                    Mathf.Cos(angleRad) * radius,
                    Mathf.Sin(angleRad) * radius,
                    0f
                );

                if (i == 0) firstPoint = point;
                else Handles.DrawLine(prev, point);

                prev = point;
            }

            Handles.DrawLine(prev, firstPoint);
        }

        public void EditorForcePreviewRebuild()
        {
            previewDirty = true;
            RebuildPreview();
            SceneView.RepaintAll();
        }

        public void EditorClearPreviewInternal()
        {
            previewCache.Clear();
            SceneView.RepaintAll();
        }
    }
}
#endif
