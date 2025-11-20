using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HexDungeon
{
    public interface IHexGenerator
    {
        IEnumerable<HexCoord> Generate(HexCoord start);
    }

    public enum GenerationMode
    {
        Shapes,
        Randomized,
    }

    public class HexRoomGenerator : MonoBehaviour
    {
        [Header("=== General ===")]
        [SerializeField] private HexOrientation orientation = HexOrientation.FlatTop;

        [Header("=== Visual ===")]
        [SerializeField, Tooltip("Note: Hex sprite must be oriented correctly. Generator does NOT auto-rotate sprites.")]
        private GameObject hexPrefab;
        [SerializeField] private float hexScale = 1f;

        [Header("=== Geometry ===")]
        [SerializeField] private float hexSize = 1f;

        [Header("=== Generation ===")]
        [SerializeField] private GenerationMode mode;

        [Header("=== Random Walk ===")]
        [SerializeField] private HexRandomGenerationType randomType;
        [SerializeField] private int rooms = 10;

        [Header("=== Shapes ===")]
        [SerializeField] private HexShapeType shapeType;
        [SerializeField] private int radius = 2;
        [SerializeField] private int corridorThickness;

#if UNITY_EDITOR
        [Header("=== Preview / Gizmos ===")]
        [SerializeField] private bool previewInEditor = true;
        [SerializeField] private Color gizmoColor = Color.blue;
        [SerializeField, Range(0.1f, 1.5f)] private float gizmoHexScale = 0.9f;
#endif

        [Header("=== Debugging ===")]
        [SerializeField, Tooltip("Works only in Play Mode")]
        private bool debugMode = false;
        [SerializeField] private float debugStepDelay = 0.1f;
        [SerializeField] private bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")]
        private int seed;

        private void Start()
        {
            if (debugMode) StartCoroutine(DebugGenerate());
            else Generate();
        }

        private IHexGenerator CreateGenerator()
        {
            switch (mode)
            {
                case GenerationMode.Shapes:
                    return new HexShapeGenerator(shapeType, radius, corridorThickness);

                case GenerationMode.Randomized:
                    return new HexRandomizedGenerator(randomType, rooms);

                default:
                    throw new ArgumentOutOfRangeException($"Unknown generation mode: {mode}");
            }
        }

        private void Generate()
        {
            if (useSeed) UnityEngine.Random.InitState(seed);

            var layout = new HexLayout(orientation, hexSize);
            var generator = CreateGenerator();

            foreach (var hex in generator.Generate(HexCoord.Zero))
                SpawnHex(layout, hex);
        }

        private IEnumerator DebugGenerate()
        {
            if (useSeed) UnityEngine.Random.InitState(seed);

            var layout = new HexLayout(orientation, hexSize);
            var generator = CreateGenerator();

            foreach (var hex in generator.Generate(HexCoord.Zero))
            {
                SpawnHex(layout, hex);
                yield return new WaitForSeconds(debugStepDelay);
            }
        }

        private void SpawnHex(HexLayout layout, HexCoord hex)
        {
            if (hexPrefab == null) return;

            Vector3 pos = layout.HexToWorld(hex);
            var instance = Instantiate(hexPrefab, pos, Quaternion.identity, transform);
            instance.transform.localScale = Vector3.one * hexScale;
        }







#if UNITY_EDITOR


        public void EditorGenerate()
        {
            EditorClear();
            Generate();
        }

        public void EditorClear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        public void EditorRandomizeSeed()
        {
            if (useSeed) seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            EditorForceRedraw();
        }

        public void EditorShowGizmosPreview()
        {
            previewDirty = true;
            RebuildPreview();
            EditorForceRedraw();
        }

        public void EditorClearPreview()
        {
            previewCache.Clear();
            EditorForceRedraw();
        }

        public void EditorForceRedraw() => SceneView.RepaintAll();



        private List<HexCoord> previewCache = new List<HexCoord>();
        private bool previewDirty = true;

        private void RebuildPreview()
        {
            previewCache.Clear();

            if (useSeed) UnityEngine.Random.InitState(seed);

            var generator = CreateGenerator();

            foreach (var hex in generator.Generate(HexCoord.Zero))
                previewCache.Add(hex);

            previewDirty = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!previewInEditor) return;
            if (!enabled) return;

            if (previewDirty)
                RebuildPreview();

            if (previewCache.Count == 0) return;

            var layout = new HexLayout(orientation, hexSize);

            Handles.color = gizmoColor;

            foreach (var hex in previewCache)
            {
                Vector3 center = transform.TransformPoint(layout.HexToWorld(hex));
                DrawHexHandle(center, layout, gizmoHexScale);
            }
        }

        private void DrawHexHandle(Vector3 center, HexLayout layout, float scale)
        {
            float radius = layout.Size * scale;

            float startAngle = orientation == HexOrientation.FlatTop ? 0f : 30f;

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
#endif
    }
}
