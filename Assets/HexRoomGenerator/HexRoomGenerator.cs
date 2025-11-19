using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace HexDungeon
{
    public enum GenerationMode
    {
        Shapes,
        Randomized,
    }
    public enum HexOrientation
    {
        FlatTop,
        PointyTop
    }

    public class HexRoomGenerator : MonoBehaviour
    {
        [Header("=== General ===")]
        [SerializeField] private HexOrientation orientation = HexOrientation.FlatTop;
        [Header("=== Visual ===")]
        [SerializeField] private GameObject hexPrefab;
        [SerializeField] private float hexScale = 1f;
        [Header("=== Geometry ===")]
        [SerializeField] private float hexSize = 1f;
        [SerializeField] private Vector2 hexSpacing;

        [Header("=== Generation ===")]
        [SerializeField] private GenerationMode mode;

        [Header("=== Random Walk ===")]
        [SerializeField] private HexRandomGenerationType randomType;
        [SerializeField] private int rooms = 10;
        
        [Header("=== Shapes ===")]
        [SerializeField] private HexShapeType shapeType;
        [SerializeField] private int radius = 2;
        [SerializeField] private int corridorThickness;

        [Header("=== Debugging ===")]
        [SerializeField, Tooltip("Works only in Play Mode")] private bool debugMode = false;
        [SerializeField] private float debugStepDelay = 0.1f;
        [SerializeField] private bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] private int seed;

        private void Start()
        {
            if (debugMode) StartCoroutine(DebugGenerate());
            else Generate();
        }

        private IEnumerable<HexCoord> GetGeneratedCoords()
        {
            if (mode == GenerationMode.Shapes)
                return HexShapeGenerator.Generate(shapeType, HexCoord.Zero, radius, corridorThickness);
            else 
                return HexRandomizedGenerator.Generate(randomType, HexCoord.Zero, rooms);
        }

        private void Generate()
        {
            if (useSeed) Random.InitState(seed);

            HashSet<HexCoord> tiles = new HashSet<HexCoord>();

            foreach (var hex in GetGeneratedCoords())
            {
                Vector3 pos = HexToWorld(hex);
                var instance = Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                if (orientation == HexOrientation.PointyTop)
                    instance.transform.rotation = Quaternion.Euler(0, 0, 90);
                instance.transform.localScale = Vector3.one * hexScale;
                tiles.Add(hex);
            }
        }

        private IEnumerator DebugGenerate()
        {
            if (useSeed) Random.InitState(seed);
            
            HashSet<HexCoord> tiles = new HashSet<HexCoord>();

            foreach (var hex in GetGeneratedCoords())
            {
                Vector3 pos = HexToWorld(hex);
                var instance = Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                if (orientation == HexOrientation.PointyTop)
                    instance.transform.rotation = Quaternion.Euler(0, 0, 90);
                instance.transform.localScale = Vector3.one * hexScale;
                tiles.Add(hex);
                yield return new WaitForSeconds(debugStepDelay);
            }
        }

        private Vector3 HexToWorld(HexCoord hex) => 
            orientation == HexOrientation.FlatTop
            ? HexToWorld_FlatTop(hex)
            : HexToWorld_PointyTop(hex);

        private Vector3 HexToWorld_FlatTop(HexCoord hex)
        {
            float size = hexSize;

            float w = size * 2f;
            float h = Mathf.Sqrt(3f) * size;

            float x = (3f * size / 2f) * hex.Q;
            float y = (h * (hex.R + hex.Q * 0.5f));

            x += hexSpacing.x * hex.Q;
            y += hexSpacing.y * hex.R;

            return new Vector3(x, y, 0);
        }

        private Vector3 HexToWorld_PointyTop(HexCoord hex)
        {
            float size = hexSize;

            float w = Mathf.Sqrt(3f) * size;
            float h = size * 2f;

            float x = w * (hex.Q + hex.R * 0.5f);
            float y = (3f * size / 2f) * hex.R;

            x += hexSpacing.x * hex.Q;
            y += hexSpacing.y * hex.R;

            return new Vector3(x, y, 0);
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
            if (useSeed)
                seed = Random.Range(int.MinValue, int.MaxValue);
        }
        public void EditorRandomizeSeedAndGenerate()
        {
            EditorRandomizeSeed();
            EditorGenerate();
        }
#endif
    }
}
