using System.Collections.Generic;

namespace HexDungeon
{
    [System.Serializable]
    public class BranchWalkGenerator : RandomizedGenerator
    {
        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            rooms.Add(start);

            HexCoord current = start;
            HexCoord? previous = null;

            for (int i = rooms.Count; i < hexCount;)
            {
                HexCoord next = GetNextStep(current, previous);

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

            foreach (HexCoord hex in rooms)
                yield return hex;
        }
    }
}
