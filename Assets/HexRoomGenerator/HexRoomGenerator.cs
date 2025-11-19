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

    public class HexRoomGenerator : MonoBehaviour
    {
        [Header("=== General ===")]
        [SerializeField] private int radius = 2;
        [SerializeField] private float hexDistance = 0.5f;
        [SerializeField] private GameObject hexPrefab;

        [Header("=== Generation ===")]
        [SerializeField] private GenerationMode mode;

        [Header("=== Random Walk ===")]
        [SerializeField] private HexRandomGenerationType randomType;
        [SerializeField] private int rooms = 10;
        
        [Header("=== Shapes ===")]
        [SerializeField] private HexShapeType shapeType;
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
                Vector3 pos = HexToWorld(hex, hexDistance);
                Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                tiles.Add(hex);
            }
        }

        private IEnumerator DebugGenerate()
        {
            if (useSeed) Random.InitState(seed);
            
            HashSet<HexCoord> tiles = new HashSet<HexCoord>();

            foreach (var hex in GetGeneratedCoords())
            {
                Vector3 pos = HexToWorld(hex, hexDistance);
                Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                tiles.Add(hex);
                yield return new WaitForSeconds(debugStepDelay);
            }
        }

        private Vector3 HexToWorld(HexCoord h, float size)
        {
            float x = size * (1.5f * h.Q);
            float y = size * (Mathf.Sqrt(3) * (h.R + h.Q * 0.5f));      
            return new Vector3(x, y, 0f);
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
