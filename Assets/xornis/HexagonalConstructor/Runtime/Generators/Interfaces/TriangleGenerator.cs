using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    [System.Serializable]
    public class TriangleGenerator : ShapeGenerator
    {
        [SerializeField, Min(1)] private int sideLength = 3;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            yield return start;

            HexCoord current = start;
            HexDirection direction = HexDirection.East;

            int segmentLength = 1;

            for (int layer = 1; layer <= sideLength; layer++)
            {
                current = current.Neighbor(direction);
                int stepsThisLayer = layer == sideLength ? segmentLength - 1 : segmentLength;

                for (int step = 0; step < stepsThisLayer; step++)
                {
                    yield return current;
                    if (step < stepsThisLayer - 1)
                        current = current.Neighbor(direction);
                }
                direction = direction.Next().Next();
                segmentLength++;
            }
        }
    }
}
