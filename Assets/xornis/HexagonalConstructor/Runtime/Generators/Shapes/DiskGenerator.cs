using UnityEngine;
using System.Collections.Generic;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class DiskGenerator : ShapeGenerator
    {
        [SerializeField, Min(1)] private int radius = 4;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius); dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    yield return new HexCoord(start.Q + dq, start.R + dr);
                }
            }
        }
    }
}
