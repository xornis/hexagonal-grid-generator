using System.Collections.Generic;

namespace HexagonalConstructor
{
    [System.Serializable]
    public class BranchWalkGenerator : RandomizedGenerator
    {
        protected override void ExecuteAlgorithm(HexCoord start, HashSet<HexCoord> rooms)
        {
            HexCoord current = start;
            HexCoord? previous = null;

            for (int i = rooms.Count; i < hexCount;)
            {
                HexCoord next = GetNextStep(current, previous, rooms);

                if (next.Equals(current))
                {
                    current = GetRandomHex(rooms);
                    previous = null;
                    continue;
                }

                rooms.Add(next);

                previous = current;
                current = next;

                i++;
            }
        }
    }
}
