using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public static class HexDungeonGraphGenerator
    {
        public static HashSet<HexCoord> RandomWalkRooms(HexCoord start, int steps)
        {
            HashSet<HexCoord> rooms = new HashSet<HexCoord>();
            rooms.Add(start);

            HexCoord current = start;
            HexCoord? previous = null;

            for (int i = 0; i < steps; i++)
            {
                List<HexDirection> dirs = new List<HexDirection>();

                for (int d = 0; d < 6; d++)
                {
                    HexDirection dir = (HexDirection)d;
                    HexCoord next = current.Neighbor(dir);

                    bool isPrev = previous.HasValue && next.Equals(previous.Value);
                    bool exists = rooms.Contains(next);

                    if (!isPrev && !exists) dirs.Add(dir);
                }

                if (dirs.Count == 0)
                {
                    current = GetRandomHex(rooms);
                    previous = null;
                    continue;
                }

                HexDirection rndDir = dirs[Random.Range(0, dirs.Count)];
                HexCoord nextHex = current.Neighbor(rndDir);

                previous = current;
                current = nextHex;

                rooms.Add(nextHex);
            }

            return rooms;
        }

        private static HexCoord GetRandomHex(HashSet<HexCoord> set)
        {
            int index = Random.Range(0, set.Count);

            foreach (HexCoord coord in set)
                if (index-- == 0) return coord;

            return HexCoord.Zero;
        }
    }
}
