using System.Collections.Generic;
using UnityEngine;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class SpiralGenerator : ShapeGenerator
    {
        [SerializeField, Min(1)] private int spiralLength = 50;
        [SerializeField] private HexDirection startDirection = HexDirection.NorthWest;
        [SerializeField, Min(1)] private int growth = 1;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            HexCoord current = start;
            yield return current;

            HexDirection direction = startDirection;

            int segmentLength = Mathf.Max(1, growth);
            int hexesPlaced = 1;

            while (hexesPlaced < spiralLength)
            {
                for (int sides = 0; sides < 3; sides++)
                {
                    for (int i = 0; i < segmentLength && hexesPlaced < spiralLength; i++)
                    {
                        current = current.Neighbor(direction);
                        yield return current;
                        hexesPlaced++;
                    }
                    direction = direction.Next();
                }
                segmentLength += growth;
            }
        }
    }
}
