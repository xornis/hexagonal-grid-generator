using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexShape
    {
        Disk, Ring, Spiral, Triangle
    }

    public class HexShapeGenerator : IHexGenerator
    {
        private readonly HexShape type;
        private readonly int radius;

        private readonly int hexCount;
        private readonly HexDirection startDirection;
        private readonly int growth;

        public HexShapeGenerator(HexShape type, int radius, int hexCount, int growth, HexDirection startDirection)
        {
            this.type = type;
            this.radius = Mathf.Max(0, radius);
            this.hexCount = Mathf.Max(1, hexCount);
            this.growth = Mathf.Max(1, growth);
            this.startDirection = startDirection;
        }

        public IEnumerable<HexCoord> Generate(HexCoord start)
        {
            HashSet<HexCoord> set = new HashSet<HexCoord>();

            switch (type)
            {
                case HexShape.Disk:
                    foreach (var hex in Disk(start, radius)) set.Add(hex);
                    break;

                case HexShape.Ring:
                    foreach (var hex in Ring(start, radius)) set.Add(hex);
                    break;

                case HexShape.Spiral:
                    foreach (var hex in Spiral(start, hexCount, growth, startDirection)) set.Add(hex);
                    break;

                case HexShape.Triangle:
                    foreach (var hex in Triangle(start, radius)) set.Add(hex);
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown generation type: {type}");
            }

            return set;
        }

        private static IEnumerable<HexCoord> Disk(HexCoord start, int radius)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius); dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    yield return new HexCoord(start.Q + dq, start.R + dr);
                }
            }
        }

        private static IEnumerable<HexCoord> Ring(HexCoord start, int radius)
        {
            if (radius <= 0)
            {
                yield return start;
                yield break;
            }

            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius);
                    dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    var hex = new HexCoord(start.Q + dq, start.R + dr);

                    if (start.Distance(hex) == radius)
                        yield return hex;
                }
            }
        }

        private static IEnumerable<HexCoord> Spiral(HexCoord start, int hexCount, int growth, HexDirection startDirection)
        {
            HexCoord current = start;
            yield return current;

            HexDirection direction = startDirection;

            int segmentLength = Mathf.Max(1, growth);
            int hexesPlaced = 1;
            const int RepeatSides = 3;

            while (hexesPlaced < hexCount)
            {
                for (int sides = 0; sides < RepeatSides; sides++)
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

        private static IEnumerable<HexCoord> Triangle(HexCoord start, int sideLength)
        {
            yield return start;

            HexCoord current = start;
            HexDirection direction = HexDirection.East;

            int segmentLength = 1;

            for (int layer = 1; layer <= sideLength; layer++)
            {
                current = current.Neighbor(direction);
                int stepsThisLayer = layer == sideLength ? segmentLength - 1 : segmentLength;

                for (int step = 0; step < stepsThisLayer; step++)
                {
                    yield return current;
                    if (step < stepsThisLayer - 1)
                        current = current.Neighbor(direction);
                }
                direction = direction.Next().Next();
                segmentLength++;
            }
        }
    }
}
