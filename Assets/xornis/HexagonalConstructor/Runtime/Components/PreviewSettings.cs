#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexagonalConstructor
{
    [ExecuteInEditMode]
    public class PreviewSettings : ContextBehaviour
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private Color hexColor = Color.yellow;
        [SerializeField, Range(0.1f, 1.5f)] private float hexScale = 0.9f;

        private List<HexCoord> previewCache = new List<HexCoord>();
        [System.NonSerialized] private bool previewDirty = true;

        public static System.Action OnForceRebuild;

        private void OnEnable()
        {   
            previewDirty = true;
            OnForceRebuild += MarkDirty;
        }

        private void OnDisable()
        {
            OnForceRebuild -= MarkDirty;
        }

        private void MarkDirty()
        {
            previewDirty = true;
            SceneView.RepaintAll();
        }

        private void RebuildPreview()
        {
            var previewGen = Context.Generation.CurrentGenerator;

            if (previewGen == null)
            {
                previewCache.Clear();
                previewDirty = false;
                return;
            }

            previewCache.Clear();

            foreach (var hex in previewGen.Generate(Context.Generation.StartHex))
                previewCache.Add(hex);

            previewDirty = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!isActive || !enabled) return;

            if (previewDirty)
                RebuildPreview();

            Handles.color = hexColor;

            foreach (var hex in previewCache)
            {
                Vector3 center = transform.TransformPoint(Context.Grid.HexLayout.HexToWorld(hex));
                DrawHexHandle(center, Context.Grid.HexLayout, hexScale);
            }
        }

        private void DrawHexHandle(Vector3 center, HexLayout layout, float scale)
        {
            float radius = layout.Size * scale;

            float startAngle = Context.Grid.HexOrientation == HexOrientation.FlatTop ? 0f : 30f;

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
    }
}
#endif
