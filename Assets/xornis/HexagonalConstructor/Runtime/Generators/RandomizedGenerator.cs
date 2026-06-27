using System.Collections.Generic;
using UnityEngine;

namespace HexagonalConstructor
{
    public abstract class RandomizedGenerator : SerializableGridGenerator
    {
        [SerializeField, Min(1)] protected int hexCount = 100;
        [SerializeField] protected bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] protected int seed;

        public bool UseSeed => useSeed;

        public override IEnumerable<HexCoord> Generate(HexCoord start)
        {
            var rooms = new HashSet<HexCoord>();

            //rooms.Clear();
            rooms.Add(start);
            
            ExecuteAlgorithm(start, rooms);

            foreach (var hex in rooms)
                yield return hex;
        }

        protected abstract void ExecuteAlgorithm(HexCoord start, HashSet<HexCoord> rooms);

        protected HexCoord GetNextStep(HexCoord current, HexCoord? previous, HashSet<HexCoord> rooms)
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

        protected HexCoord GetRandomHex(HashSet<HexCoord> set)
        {
            int index = Random.Range(0, set.Count);

            foreach (HexCoord coord in set)
                if (index-- == 0) return coord;

            return HexCoord.Zero;
        }
    }
}
