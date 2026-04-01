#if UNITY_EDITOR
using UnityEngine;

namespace HexagonalConstructor
{
    public class DebugSettings : MonoBehaviour
    {
        [SerializeField] private bool isActive = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;

        private GeneratorContext context;

        public bool IsDebugMode => isActive;
        public float StepDelay => stepDelay;

        private void Awake()
        {
            context = GetComponent<GeneratorContext>();
        }

        private void Start()
        {
            if (isActive && context != null)
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