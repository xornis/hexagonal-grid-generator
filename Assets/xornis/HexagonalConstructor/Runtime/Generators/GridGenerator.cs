using System.Collections;
using UnityEngine;

namespace HexagonalConstructor
{
    public enum GenerationMode
    {
        Shapes, Randomized
    };

    [RequireComponent(typeof(GeneratorContext))]
    public class GridGenerator : MonoBehaviour
    {
        private GeneratorContext context;

        private void Awake()
        {
            context = GetComponent<GeneratorContext>();
        }

        private void Start()
        {
            Generate();
        }

        public void Generate()
        {
            foreach (var hex in context.Generation.CurrentGenerator.Generate(context.Generation.StartHex))
                SpawnHex(hex);
        }

        public IEnumerator GenerateWithDelay(float delay)
        {
            foreach (var hex in context.Generation.CurrentGenerator.Generate(context.Generation.StartHex))
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
            Vector3 pos = context.Grid.HexLayout.HexToWorld(hex);
            var instance = Instantiate(context.Grid.HexPrefab, pos, Quaternion.identity, transform);
            instance.transform.localScale = Vector3.one * context.Grid.HexScale;
        }
    }
}