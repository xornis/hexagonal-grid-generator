using System.Collections.Generic;
using UnityEngine;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class RhombusGenerator : ShapeGenerator
    {
        [SerializeField, Min(2)] private int sideLength = 3;
        [SerializeField] private HexDirection startDirection = HexDirection.SouthWest;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            HexCoord current = start;
            yield return current;

            HexDirection direction = startDirection; 

            for (int size = 2; size <= sideLength; size++)
            {
                for (int side = 0; side < 3; side++)
                {
                    int steps = (side == 0) ? 1 : (size - 1);

                    for (int step = 0; step < steps; step++)
                    {
                        current = current.Neighbor(direction);
                        yield return current;
                    }

                    if (side == 0) direction = direction.Next().Next();
                    else if (side == 1) direction = direction.Next();
                }
            }
        }
    }
}
