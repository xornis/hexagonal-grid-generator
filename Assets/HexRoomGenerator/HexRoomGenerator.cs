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
        [SerializeField] private bool debugMode = false;
        [SerializeField] private float debugStepDelay = 0.1f;

        private void Start()
        {
            if (debugMode)
                StartCoroutine(DebuggedGenerator());
            else
                Generator();
        }

        private IEnumerator DebuggedGenerator()
        {
            if (mode == GenerationMode.Shapes)
            {
                foreach (var hex in HexShapeGenerator.Generate(shapeType, HexCoord.Zero, radius, corridorThickness))
                {
                    Vector3 pos = HexToWorld(hex, hexDistance);
                    Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                    yield return new WaitForSeconds(debugStepDelay);
                }
            }
            else if (mode == GenerationMode.Randomized)
            {
                foreach (var hex in HexRandomizedGenerator.Generate(randomType, HexCoord.Zero, rooms))
                {
                    Vector3 pos = HexToWorld(hex, hexDistance);
                    Instantiate(hexPrefab, pos, Quaternion.identity, transform);
                    yield return new WaitForSeconds(debugStepDelay);
                }
            }

        }

        private void Generator()
        {
            HashSet<HexCoord> tiles = new HashSet<HexCoord>();

            if (mode == GenerationMode.Shapes)
                foreach (var hex in HexShapeGenerator.Generate(shapeType, HexCoord.Zero, radius, corridorThickness))
                    tiles.Add(hex);

            else if (mode == GenerationMode.Randomized)
                foreach (var hex in HexRandomizedGenerator.Generate(randomType, HexCoord.Zero, rooms))
                    tiles.Add(hex);

            foreach (var hex in tiles)
            {
                Vector3 pos = HexToWorld(hex, hexDistance);
                Instantiate(hexPrefab, pos, Quaternion.identity, transform);
            }
        }

        private Vector3 HexToWorld(HexCoord h, float size)
        {
            float x = size * (1.5f * h.Q);
            float y = size * (Mathf.Sqrt(3) * (h.R + h.Q * 0.5f));
            return new Vector3(x, y, 0f);
        }
    }
}
