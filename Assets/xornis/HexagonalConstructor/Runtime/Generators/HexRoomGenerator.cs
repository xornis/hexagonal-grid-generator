using System;
using System.Collections;
using UnityEngine;

namespace HexDungeon
{
    public enum GenerationMode
    {
        Shapes, Randomized
    };

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

        [SerializeField] private GenerationMode generationMode;
        [SerializeField, SerializeReference] private ShapeGenerator shapeGenerator;
        [SerializeField, SerializeReference] private RandomizedGenerator randomizedGenerator;

        public SerializableHexGenerator CurrentGenerator
        {
            get => generationMode == GenerationMode.Randomized 
                ? randomizedGenerator
                : shapeGenerator;
        }

        #endregion Generation Settings

#if UNITY_EDITOR
        #region Editor Preview
        [SerializeField] private bool previewIsActive = true;
        [SerializeField] private Color previewHexColor = Color.blue;
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

        private void Generate()
        {
            foreach (var hex in CurrentGenerator.Generate(GetGeneratorSettings.startHex))
                SpawnHex(hex);
        }

        private IEnumerator DebugGenerate()
        {
            foreach (var hex in CurrentGenerator.Generate(GetGeneratorSettings.startHex))
            {
                SpawnHex(hex);
                yield return new WaitForSeconds(stepDelay);
            }
        }

        private void SpawnHex(HexCoord hex)
        {
            if (hexPrefab == null) return;

            Vector3 pos = GetGeneratorSettings.hexLayout.HexToWorld(hex);
            var instance = Instantiate(hexPrefab, pos, Quaternion.identity, transform);
            instance.transform.localScale = Vector3.one * hexScale;
        }

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
#endif
        
        public struct GeneratorSettings
        {
            public HexLayout hexLayout;
            public HexCoord startHex;
            public float hexScale;
            public HexOrientation hexOrientation;

            public bool previewIsActive;
            public Color previewHexColor;
            public float previewHexScale;
        }

        public GeneratorSettings GetGeneratorSettings
        {
            get => new GeneratorSettings
            {
                hexLayout = new HexLayout(hexOrientation, hexRadius),
                startHex = new HexCoord(startAxial.x, startAxial.y),
                hexScale = hexScale,
                hexOrientation = hexOrientation,

                previewIsActive = previewIsActive,
                previewHexColor = previewHexColor,
                previewHexScale = previewHexScale
            };
        }
    }

}