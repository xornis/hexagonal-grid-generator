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
        private HexGridSettings gridSettings;
        private HexGenerationSettings generationSettings;

        #region Generator Debug 
        [SerializeField] private bool debugMode = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;
        #endregion Generator Debug

        private void Awake()
        {
            gridSettings = GetComponent<HexGridSettings>();
            generationSettings = GetComponent<HexGenerationSettings>();
        }

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
            foreach (var hex in generationSettings.CurrentGenerator.Generate(generationSettings.StartHex))
                SpawnHex(hex);
        }

        private IEnumerator DebugGenerate()
        {
            foreach (var hex in generationSettings.CurrentGenerator.Generate(generationSettings.StartHex))
            {
                SpawnHex(hex);
                yield return new WaitForSeconds(stepDelay);
            }
        }

        private void SpawnHex(HexCoord hex)
        {
            if (gridSettings.HexPrefab == null) return;

            Vector3 pos = gridSettings.HexLayout.HexToWorld(hex);
            var instance = Instantiate(gridSettings.HexPrefab, pos, Quaternion.identity, transform);
            instance.transform.localScale = Vector3.one * gridSettings.HexScale;
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
    }
}