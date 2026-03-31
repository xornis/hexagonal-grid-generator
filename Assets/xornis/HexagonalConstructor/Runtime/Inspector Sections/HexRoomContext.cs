using UnityEngine;

namespace HexDungeon
{
    [RequireComponent(typeof(HexGridSettings))]
    public class HexRoomContext : MonoBehaviour
    {
        private HexGridSettings gridSettings;

        public HexGridSettings Grid
        {
            get
            {
                if (gridSettings == null)
                    gridSettings = GetComponent<HexGridSettings>();
                return gridSettings;
            }
        }
    }
}
