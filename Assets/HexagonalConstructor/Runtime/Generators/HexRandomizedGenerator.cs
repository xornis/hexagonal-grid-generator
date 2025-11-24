using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexRandomGenerationType
    {
        RandomWalk
    }

    public class HexRandomizedGenerator : IHexGenerator
    {
        private readonly HexRandomGenerationType type;
        private readonly int desiredRoomCount;

        public HexRandomizedGenerator(HexRandomGenerationType type, int desiredRoomCount)
        {
            this.type = type;
            this.desiredRoomCount = Mathf.Max(1, desiredRoomCount);
        }

        public IEnumerable<HexCoord> Generate(HexCoord start)
        {
            switch (type)
            {
                case HexRandomGenerationType.RandomWalk: 
                    foreach (var hex in RandomWalk(start, desiredRoomCount))
                        yield return hex;
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown generation type: {type}");
            }
        }

        private static IEnumerable<HexCoord> RandomWalk(HexCoord start, int desiredRoomsCount)
        {
            HashSet<HexCoord> rooms = new HashSet<HexCoord>() { start };

            HexCoord current = start;
            HexCoord? previous = null;

            for (int i = rooms.Count; i < desiredRoomsCount;)
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

            foreach (HexCoord hex in rooms)
                yield return hex;
        }

        private static HexCoord GetNextStep(HexCoord current, HexCoord? previous, HashSet<HexCoord> rooms)
        {
            List<HexDirection> availableDirs = new List<HexDirection>();

            for (int d = 0; d < 6; d++)
            {
                HexDirection dir = (HexDirection)d;
                HexCoord next = current.Neighbor(dir);

                bool isPrev = previous.HasValue && next.Equals(previous.Value);
                bool exists = rooms.Contains(next);

                if (!isPrev && !exists) availableDirs.Add(dir);
            }

            if (availableDirs.Count == 0) return current;

            HexDirection rndDir = availableDirs[UnityEngine.Random.Range(0, availableDirs.Count)];
            return current.Neighbor(rndDir);
        }

        private static HexCoord GetRandomHex(HashSet<HexCoord> set)
        {
            int index = UnityEngine.Random.Range(0, set.Count);

            foreach (HexCoord coord in set)
                if (index-- == 0) return coord;

            return HexCoord.Zero;
        }
    }
}
