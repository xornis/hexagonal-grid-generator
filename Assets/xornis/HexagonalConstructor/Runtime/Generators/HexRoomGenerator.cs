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

    public class HexRoomGenerator : MonoBehaviour
    {
        #region Grid Settings

        #region Tile Visuals 
        [SerializeField, Tooltip("Note: Hex sprite must be oriented correctly. Generator does NOT auto-rotate sprites.")] private GameObject hexPrefab;
        [SerializeField] private float hexScale = 1f;
        #endregion Tile Visuals

        #region Tile Geometry
        [SerializeField] private HexOrientation hexOrientation = HexOrientation.FlatTop;
        [SerializeField] private float hexRadius = 1f;
        #endregion Tile Geometry

        #endregion Grid Settings

        #region Generation Settings
        [SerializeField, Tooltip("Axial coordinates (Q, R) of the starting hex. \nX = Q \nY = R")] private Vector2Int startAxial;

        #region Random Generation
        [SerializeField] private HexRandomGenerationAlgorithm randomAlgorithm;
        [SerializeField] private int roomCount = 100;
        [SerializeField] private bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] private int seed;
        #endregion Random Generation

        [SerializeReference] private SerializableHexGenerator generator;

        #endregion Generation Settings

#if UNITY_EDITOR
        #region Editor Preview
        [SerializeField] private bool enablePreview = true;
        [SerializeField] private Color previewColor = Color.blue;
        [SerializeField, Range(0.1f, 1.5f)] private float previewHexScale = 0.9f;
        #endregion Editor Preview
#endif

        #region Generator Debug 
        [SerializeField] private bool debugMode = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;
        #endregion Generator Debug

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

        private void Reset()
        {
            generator = new DiskGenerator();
        }

        private IHexGenerator CreateGenerator() => generator;

        private void Generate()
        {
            if (useSeed) UnityEngine.Random.InitState(seed);

            var layout = new HexLayout(hexOrientation, hexRadius);
            var gen = CreateGenerator();

            HexCoord startHex = new HexCoord(startAxial.x, startAxial.y);

            foreach (var hex in gen.Generate(startHex))
                SpawnHex(layout, hex);
        }

        private IEnumerator DebugGenerate()
        {
            if (useSeed) UnityEngine.Random.InitState(seed);

            var layout = new HexLayout(hexOrientation, hexRadius);
            var gen = CreateGenerator();

            HexCoord startHex = new HexCoord(startAxial.x, startAxial.y);

            foreach (var hex in gen.Generate(startHex))
            {
                SpawnHex(layout, hex);
                yield return new WaitForSeconds(stepDelay);
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
            if (generator == null)
            {
                previewDirty = false;
                return;
            }

            previewCache.Clear();
            if (useSeed) UnityEngine.Random.InitState(seed);

            var gen = CreateGenerator();

            HexCoord startHex = new HexCoord(startAxial.x, startAxial.y);

            foreach (var hex in generator.Generate(startHex))
                previewCache.Add(hex);
        }

        private void OnDrawGizmosSelected()
        {
            if (!enablePreview) return;
            if (!enabled) return;

            if (previewDirty)
                RebuildPreview();

            var layout = new HexLayout(hexOrientation, hexRadius);
            Handles.color = previewColor;

            foreach (var hex in previewCache)
            {
                Vector3 center = transform.TransformPoint(layout.HexToWorld(hex));
                DrawHexHandle(center, layout, previewHexScale);
            }
        }

        private void DrawHexHandle(Vector3 center, HexLayout layout, float scale)
        {
            float radius = layout.Size * scale;

            float startAngle = hexOrientation == HexOrientation.FlatTop ? 0f : 30f;

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