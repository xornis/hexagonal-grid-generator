using UnityEngine;

namespace HexDungeon
{
    [RequireComponent(typeof(HexGridSettings))]
    [RequireComponent(typeof(HexGenerationSettings))]
    [RequireComponent(typeof(HexRoomGenerator))]
    public class HexRoomContext : MonoBehaviour
    {
        private HexGridSettings gridSettings;
        private HexGenerationSettings generationSettings;
        private HexRoomGenerator generator;

#if UNITY_EDITOR
        private HexDebugSettings debugSettings;
        private HexPreviewSettings previewSettings;
#endif

        public HexGridSettings Grid
        {
            get
            {
                if (gridSettings == null)
                    gridSettings = GetComponent<HexGridSettings>();
                return gridSettings;
            }
        }

        public HexGenerationSettings Generation
        {
            get
            {
                if (generationSettings == null)
                    generationSettings = GetComponent<HexGenerationSettings>();
                return generationSettings;
            }
        }

        public HexRoomGenerator Generator
        {
            get
            {
                if (generator == null)
                    generator = GetComponent<HexRoomGenerator>();
                return generator;
            }
        }

#if UNITY_EDITOR
        public HexDebugSettings Debug
        {
            get
            {
                if (debugSettings == null)
                    debugSettings = GetComponent<HexDebugSettings>();
                return debugSettings;
            }
        }

        public HexPreviewSettings Preview
        {
            get
            {
                if (previewSettings == null)
                    previewSettings = GetComponent<HexPreviewSettings>();
                return previewSettings;
            }
        }
#endif
    }
}
