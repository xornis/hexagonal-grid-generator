using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    [System.Serializable]
    public class SpiralGenerator : IHexGenerator
    {
        [SerializeField, Min(1)] private int hexCount;
        [SerializeField] private HexDirection startDirection;
        [SerializeField, Min(1)] private int growth;

        public IEnumerable<HexCoord> Generate(HexCoord start)
        {
            HexCoord current = start;
            yield return current;

            HexDirection direction = startDirection;

            int segmentLength = Mathf.Max(1, growth);
            int hexesPlaced = 1;

            while (hexesPlaced < hexCount)
            {
                for (int sides = 0; sides < 3; sides++)
                {
                    for (int i = 0; i < segmentLength && hexesPlaced < hexCount; i++)
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
