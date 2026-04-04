using System.Collections;
using UnityEngine;

namespace HexagonalConstructor
{
    public enum GenerationMode
    {
        Shapes, Randomized
    };

    [RequireComponent(typeof(GeneratorContext))]
    public class GridGenerator : ContextBehaviour
    {
        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            foreach (var hex in Context.Generation.CurrentGenerator.Generate(Context.Generation.StartHex))
                SpawnHex(hex);
        }

        public IEnumerator GenerateWithDelay(float delay)
        {
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