using UnityEngine;
using System.Collections.Generic;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class RingGenerator : ShapeGenerator
    {
        [SerializeField, Min(1)] private int radius = 4;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius);
                    dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    var hex = new HexCoord(start.Q + dq, start.R + dr);

                    if (start.Distance(hex) == radius)
                        yield return hex;
                }
            }
        }
    }
}
