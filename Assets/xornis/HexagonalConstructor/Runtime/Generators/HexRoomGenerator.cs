using System.Collections;
using UnityEngine;

namespace HexDungeon
{
    public enum GenerationMode
    {
        Shapes, Randomized
    };

    [RequireComponent(typeof(HexRoomContext))]
    public class HexRoomGenerator : MonoBehaviour
    {
        private HexRoomContext context;

        private void Awake()
        {
            context = GetComponent<HexRoomContext>();
        }

        private void Start()
        {
            if (context.Debug != null && context.Debug.IsDebugMode)
            {
                context.Debug.EditorClearInternal();
                StartCoroutine(DebugGenerate());
            }
            else
                Generate();
        }

        private void Generate()
        {
            foreach (var hex in context.Generation.CurrentGenerator.Generate(context.Generation.StartHex))
                SpawnHex(hex);
        }

        private IEnumerator DebugGenerate()
        {
            foreach (var hex in context.Generation.CurrentGenerator.Generate(context.Generation.StartHex))
            {
                SpawnHex(hex);
                yield return new WaitForSeconds(context.Debug.StepDelay);
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