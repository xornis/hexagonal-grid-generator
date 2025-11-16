using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public class HexRoomGenerator : MonoBehaviour
    {
        public int radius = 2;
        public float hexDistance = 0.5f;
        public GameObject hexPrefab;
        public HexShapeType shapeType;
        public int corridorThickness;

        private void Start()
        {
            HashSet<HexCoord> tiles = new HashSet<HexCoord>();

            foreach (var hex in HexGridShape.Generate(shapeType, HexCoord.Zero, radius, corridorThickness))
                tiles.Add(hex);
            
            foreach (var hex in tiles)
            {
                Vector3 pos = HexToWorld(hex, hexDistance);
                Instantiate(hexPrefab, pos, Quaternion.identity, transform);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                Gizmos.color = Color.yellow;

                foreach (var hex in HexGridShape.Generate(shapeType, HexCoord.Zero, radius, corridorThickness))
                {
                    Vector3 pos = HexToWorld(hex, hexDistance);
                    Gizmos.DrawWireSphere(pos, 0.75f * hexDistance);
                }
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
