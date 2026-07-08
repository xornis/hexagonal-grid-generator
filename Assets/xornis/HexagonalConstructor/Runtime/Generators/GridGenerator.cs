using System.Collections;
using UnityEngine;

namespace HexagonalConstructor
{
    public enum GenerationMode
    {
        Shapes, Randomized
    };

    public class GridGenerator : ContextBehaviour
    {
        private void Start()
        {
            if (Context == null || Context.Grid == null || Context.Generation == null)
            {
                Debug.LogError(
                    $"[GridGenerator] Missing required components on '{gameObject.name}'. " +
                    "Add GeneratorContext, GridSettings, and GenerationSettings.",
                    this
                );
                enabled = false;
                return;
            }

#if UNITY_EDITOR
            if (Context.Debug != null && Context.Debug.IsDebugMode)
                return;
#endif

            Generate();
        }

        private bool IsReadyToGenerate()
        {
            bool isReady = true;

            if (Context.Grid.HexPrefab == null)
            {
                Debug.LogError("[GridGenerator] Generation Failed! You must assign a Hex Prefab inside Grid Settings.");
                isReady = false;
            }
            
            if (Context.Generation.CurrentGenerator == null)
            {
                Debug.LogError("[GridGenerator] Generation Failed! You must choose a specific Generator Type in the Generation Settings.");
                isReady = false;
            }

            return isReady;
        }

        public void Generate()
        {
            if (!IsReadyToGenerate()) return;

            try
            {
                foreach (var hex in Context.Generation.CurrentGenerator.Generate(Context.Generation.StartHex))
                    SpawnHex(hex);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"An unexpected error occurred: {ex.Message}");
            }
        }

        public IEnumerator GenerateWithDelay(float delay)
        {
            if (!IsReadyToGenerate()) yield break;

            foreach (var hex in Context.Generation.CurrentGenerator.Generate(Context.Generation.StartHex))
            {
                SpawnHex(hex);
                yield return new WaitForSeconds(delay);
            }
        }

        public void ClearGeneration()
        {
            StopAllCoroutines();

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                DestroyImmediate(transform.GetChild(i).gameObject);
#else
                Destroy(transform.GetChild(i).gameObject);
#endif
            }
        }

        private void SpawnHex(HexCoord hex)
        {
            Vector3 pos = Context.Grid.HexLayout.HexToWorld(hex);
            var instance = Instantiate(Context.Grid.HexPrefab, pos, Quaternion.identity, transform);
            instance.transform.localScale = Vector3.one * Context.Grid.HexScale;
        }
    }
}