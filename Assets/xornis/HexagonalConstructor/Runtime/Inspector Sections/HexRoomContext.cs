using UnityEngine;

namespace HexDungeon
{
    [RequireComponent(typeof(HexGridSettings))]
    public class HexRoomContext : MonoBehaviour
    {
        private HexGridSettings gridSettings;
        private HexGenerationSettings generationSettings;

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
    }
}
