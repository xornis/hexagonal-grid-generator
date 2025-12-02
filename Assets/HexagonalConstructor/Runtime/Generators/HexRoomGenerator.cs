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
        #region General

        #region Visual
        [SerializeField, Tooltip("Note: Hex sprite must be oriented correctly. Generator does NOT auto-rotate sprites.")] private GameObject hexPrefab;
        [SerializeField] private float hexScale = 1f;
        #endregion Visual

        #region Geometry
        [SerializeField] private HexOrientation orientation = HexOrientation.FlatTop;
        [SerializeField] private float hexSize = 1f;
        #endregion Geometry

        #endregion General

        #region Generation
        [SerializeField] private GenerationMode mode;

        #region Randomized
        [SerializeField] private HexRandomGenerationType randomType;
        [SerializeField] private int rooms = 100;
        [SerializeField] private bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] private int seed;
        #endregion Randomized

        #region Shapes
        [SerializeField] private HexShapeType shapeType;

        #region Disk, Ring
        [SerializeField] private int radius = 2;
        [SerializeField] private int corridorThickness;
        #endregion Disk, Ring

        #region Spiral
        [SerializeField] private int hexCount = 200;
        [SerializeField] private HexDirection startDirection;
        [SerializeField] private int growth = 1;
        #endregion Spiral

        #endregion Shapes

        #endregion Generation

#if UNITY_EDITOR
        #region Preview
        [SerializeField] private bool previewInEditor = true;
        [SerializeField] private Color gizmoColor = Color.blue;
        [SerializeField, Range(0.1f, 1.5f)] private float gizmoHexScale = 0.9f;
        #endregion Preview
#endif

        #region Debug 
        [SerializeField] private bool debugMode = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float hexGenerationDelay = 0.1f;
        #endregion Debug

        private void Start()
        {
            if (debugMode)
            {
#if UNITY_EDITOR
                EditorClearInternal();
#endif
                StartCoroutine(DebugGenerate());
            }
            else Generate();
        }

        private IHexGenerator CreateGenerator()
        {
            switch (mode)
            {
                case GenerationMode.Shapes:
                    return new HexShapeGenerator(shapeType, radius, corridorThickness, hexCount, growth, startDirection);

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
                yield return new WaitForSeconds(hexGenerationDelay);
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

#if UNITY_EDITOR

        public void EditorGenerateInternal()
        {
            EditorClearInternal();
            StopAllCoroutines();
            if (Application.isPlaying) StartCoroutine(DebugGenerate());
            else Generate();
        }

        public void EditorClearInternal()
        {
            StopAllCoroutines();
            
            for (int i = transform.childCount - 1; i >= 0; i--) 
                DestroyImmediate(transform.GetChild(i).gameObject);
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

        public void EditorRandomizeSeedInternal()
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
#endif
    }
}