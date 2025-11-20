using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
        [SerializeField, Tooltip("Note: Hex sprite must be oriented to match selected grid orientation (FlatTop / PointyTop). Generator does not rotate sprites automatically.")]
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
            if (useSeed)
                seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
        public void EditorRandomizeSeedAndGenerate()
        {
            EditorRandomizeSeed();
            EditorGenerate();
        }
#endif
    }
}
