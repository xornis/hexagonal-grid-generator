using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public abstract class RandomizedGenerator : SerializableHexGenerator
    {
        [SerializeField, Min(1)] protected int hexCount = 100;
        [SerializeField] protected bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] protected int seed;

        protected static HashSet<HexCoord> rooms = new HashSet<HexCoord>();

        protected static HexCoord GetNextStep(HexCoord current, HexCoord? previous)
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

            HexDirection rndDir = availableDirs[Random.Range(0, availableDirs.Count)];
            return current.Neighbor(rndDir);
        }

        protected static HexCoord GetRandomHex(HashSet<HexCoord> set)
        {
            int index = Random.Range(0, set.Count);

            foreach (HexCoord coord in set)
                if (index-- == 0) return coord;

            return HexCoord.Zero;
        }

        public void RandomizeSeed() => Random.InitState(seed);
    }
}
