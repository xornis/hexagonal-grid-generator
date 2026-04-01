#if UNITY_EDITOR
using UnityEngine;

namespace HexagonalConstructor
{
    public class HexDebugSettings : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;

        private HexRoomContext context;

        public bool IsDebugMode => debugMode;
        public float StepDelay => stepDelay;

        private void Awake()
        {
            context = GetComponent<HexRoomContext>();
        }

        private void Start()
        {
            if (debugMode && context != null)
            {
                context.Generator.ClearGeneration();
                context.Generator.StartCoroutine(context.Generator.GenerateWithDelay(stepDelay));
            }                                           
        }

        public void EditorGenerate()
        {
            if (context == null || context.Generator == null) return;

            context.Generator.ClearGeneration();
            context.Generator.StopAllCoroutines();

            if (Application.isPlaying) 
                context.StartCoroutine(context.Generator.GenerateWithDelay(stepDelay));
            else 
                context.Generator.Generate();
        }

        public void EditorClear()
        {
            if (context != null && context.Generator != null)
                context.Generator.ClearGeneration();
        }
    }
}
#endif