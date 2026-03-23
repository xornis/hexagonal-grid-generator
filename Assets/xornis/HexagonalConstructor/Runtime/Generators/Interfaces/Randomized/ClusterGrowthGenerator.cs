using System.Collections.Generic;

namespace HexDungeon
{
    [System.Serializable]
    public class ClusterGrowthGenerator : RandomizedGenerator
    {
        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            rooms.Add(start);

            for (int i = rooms.Count; i < hexCount;)
            {
                HexCoord current = GetRandomHex(rooms);

                List<HexDirection> availableDirs = new List<HexDirection>();

                for (int d = 0; d < 6; d++)
                {
                    var dir = (HexDirection)d;
                    var next = current.Neighbor(dir);

                    if (!rooms.Contains(next))
                        availableDirs.Add(dir);
                }

                if (availableDirs.Count == 0) continue;

                HexDirection rndDir = availableDirs[UnityEngine.Random.Range(0, availableDirs.Count)];
                HexCoord nextHex = current.Neighbor(rndDir);

                rooms.Add(nextHex);
                i++;
            }

            foreach (var hex in rooms)
                yield return hex;
        }
    }
}