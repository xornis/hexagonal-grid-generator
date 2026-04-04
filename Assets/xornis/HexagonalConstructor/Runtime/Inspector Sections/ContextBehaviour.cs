using UnityEngine;

namespace HexagonalConstructor
{
    public abstract class ContextBehaviour : MonoBehaviour
    {
        private GeneratorContext context;

        protected GeneratorContext Context
        {
            get
            {
                if (context == null)
                    context = GetComponent<GeneratorContext>();
                return context;
            }
        }
    }
}