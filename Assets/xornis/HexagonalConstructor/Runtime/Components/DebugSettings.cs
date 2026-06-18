#if UNITY_EDITOR
using UnityEngine;

namespace HexagonalConstructor
{
    public class DebugSettings : ContextBehaviour
    {
        [SerializeField] private bool isActive = false;
        [SerializeField, Tooltip("Works only in Play Mode")] private float stepDelay = 0.1f;

        public bool IsDebugMode => isActive;
        public float StepDelay => stepDelay;

        private void Start()
        {
            if (isActive && Context != null)
            {
                Context.Generator.ClearGeneration();
                Context.Generator.StartCoroutine(Context.Generator.GenerateWithDelay(stepDelay));
            }                                           
        }

        public void EditorGenerate()
        {
            if (Context == null || Context.Generator == null) return;

            Context.Generator.ClearGeneration();
            Context.Generator.StopAllCoroutines();

            if (Application.isPlaying) 
                Context.StartCoroutine(Context.Generator.GenerateWithDelay(stepDelay));
            else 
                Context.Generator.Generate();
        }

        public void EditorClear()
        {
            if (Context != null && Context.Generator != null)
                Context.Generator.ClearGeneration();
        }
    }
}
#endif