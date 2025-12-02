using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexShapeType
    {
        Disk, Ring, Spiral, TwoRoomsWithCorridor,
    }

    public class HexShapeGenerator : IHexGenerator
    {
        private readonly HexShapeType type;
        private readonly int radius;
        private readonly int corridorThickness;

        private readonly int hexCount;
        private readonly HexDirection startDirection;
        private readonly int growth;

        public HexShapeGenerator(HexShapeType type, int radius, int corridorThickness, int hexCount, int growth, HexDirection startDirection)
        {
            this.type = type;
            this.radius = Mathf.Max(0, radius);
            this.corridorThickness = Mathf.Max(1, corridorThickness);
            this.hexCount = Mathf.Max(1, hexCount);
            this.growth = Mathf.Max(1, growth);
            this.startDirection = startDirection;
        }

        public IEnumerable<HexCoord> Generate(HexCoord start)
        {
            HashSet<HexCoord> set = new HashSet<HexCoord>();

            switch (type)
            {
                case HexShapeType.Disk:
                    foreach (var hex in Disk(start, radius)) set.Add(hex);
                    break;

                case HexShapeType.Ring:
                    foreach (var hex in Ring(start, radius)) set.Add(hex);
                    break;

                case HexShapeType.Spiral:
                    foreach (var hex in Spiral(start, hexCount, growth, startDirection)) set.Add(hex);
                    break;

                case HexShapeType.TwoRoomsWithCorridor:
                    foreach (var hex in TwoRoomsWithCorridor(start, radius, corridorThickness)) set.Add(hex);
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

        private static IEnumerable<HexCoord> Corridor(HexCoord start, HexCoord end, int thickness)
        {
            foreach (var hex in CorridorLine(start, end))
            {
                yield return hex;

                if (thickness <= 1) continue;

                for (int ring = 1; ring < thickness; ring++)
                    foreach (var thickHex in Ring(hex, ring))
                        yield return thickHex;
            }
        }

        private static IEnumerable<HexCoord> CorridorLine(HexCoord start, HexCoord end)
        {
            int steps = start.Distance(end);

            Vector3 cubeA = HexGeometryUtils.AxialToCube(start);
            Vector3 cubeB = HexGeometryUtils.AxialToCube(end);

            for (int i = 0; i <= steps; i++)
            {
                float t = steps == 0 ? 0 : (float)i / steps;

                float x = Mathf.Lerp(cubeA.x, cubeB.x, t);
                float y = Mathf.Lerp(cubeA.y, cubeB.y, t);
                float z = Mathf.Lerp(cubeA.z, cubeB.z, t);

                Vector3 rounded = HexGeometryUtils.CubeRound(new Vector3(x, y, z));

                yield return HexGeometryUtils.CubeToAxial(rounded);
            }
        }

        private static IEnumerable<HexCoord> TwoRoomsWithCorridor(HexCoord start, int radius, int thickness)
        {
            HexCoord roomA = start;
            HexCoord roomB = new HexCoord(start.Q + radius * 3, start.R - radius);

            foreach (var hex in Disk(roomA, radius)) yield return hex;
            foreach (var hex in Disk(roomB, Mathf.Max(1, radius - 1))) yield return hex;
            foreach (var hex in Corridor(roomA, roomB, thickness)) yield return hex;
        }
    }
}
