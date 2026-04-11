using UnityEngine;

namespace HexagonalConstructor
{
    public class GeneratorContext : MonoBehaviour
    {
        private GridSettings gridSettings;
        private GenerationSettings generationSettings;
        private GridGenerator generator;

#if UNITY_EDITOR
        private DebugSettings debugSettings;
        private PreviewSettings previewSettings;
#endif

        public GridSettings Grid
        {
            get
            {
                if (gridSettings == null)
                    gridSettings = GetComponent<GridSettings>();
                return gridSettings;
            }
        }

        public GenerationSettings Generation
        {
            get
            {
                if (generationSettings == null)
                    generationSettings = GetComponent<GenerationSettings>();
                return generationSettings;
            }
        }

        public GridGenerator Generator
        {
            get
            {
                if (generator == null)
                    generator = GetComponent<GridGenerator>();
                return generator;
            }
        }

#if UNITY_EDITOR
        public DebugSettings Debug
        {
            get
            {
                if (debugSettings == null)
                    debugSettings = GetComponent<DebugSettings>();
                return debugSettings;
            }
        }

        public PreviewSettings Preview
        {
            get
            {
                if (previewSettings == null)
                    previewSettings = GetComponent<PreviewSettings>();
                return previewSettings;
            }
        }
#endif
    }
}
