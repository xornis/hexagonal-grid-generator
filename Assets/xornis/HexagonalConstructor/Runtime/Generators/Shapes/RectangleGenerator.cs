using System.Collections.Generic;
using UnityEngine;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class RectangleGenerator : ShapeGenerator
    {
        [SerializeField, Min(3)] private int width = 4;
        [SerializeField, Min(3)] private int height = 4;
        [SerializeField] private bool isPointy = true;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            // isPointy is a temporary and unnecessary bool. In the future, I need more robust way to get the hex orientation, this is necessary here.

            int sizeA = isPointy ? height : width;
            int sizeB = isPointy ? width : height;

            int startA = isPointy ? start.R : start.Q;
            int startB = isPointy ? start.Q : start.R;

            int limitAStart = startA - (sizeA / 2);
            int limitAEnd = startA + ((sizeA - 1) / 2);

            for (int a = limitAStart; a <= limitAEnd; a++)
            {
                int bOffset = Mathf.FloorToInt((a - startA) / 2f);

                int bStart = startB - (sizeB / 2) - bOffset;
                int bEnd = startB + ((sizeB - 1) / 2) - bOffset;

                for (int b = bStart; b <= bEnd; b++)
                    yield return isPointy ? new HexCoord(b, a) : new HexCoord(a, b);
            }
        }
    }
}
