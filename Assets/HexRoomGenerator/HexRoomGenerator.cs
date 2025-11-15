using UnityEngine;

namespace HexDungeon
{
    public class HexRoomGenerator : MonoBehaviour
    {
        public int radius = 2;
        public float hexSize = 1f;
        public GameObject hexPrefab;


        private void Start()
        {
            foreach (var hex in HexCoord.Zero.Disk(radius))
            {
                Vector3 pos = HexToWorld(hex, hexSize);
                Instantiate(hexPrefab, pos, Quaternion.identity, transform);
            }
        }

        private Vector3 HexToWorld(HexCoord h, float size)
        {
            float x = size * (1.5f * h.Q);
            float y = size * (Mathf.Sqrt(3) * (h.R + h.Q * 0.5f));
            return new Vector3(x, y, 0f);
        }
    }
}
