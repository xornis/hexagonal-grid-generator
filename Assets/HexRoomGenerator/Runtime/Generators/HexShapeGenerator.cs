using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexShapeType
    {
        Disk, Ring, TwoRoomsWithCorridor,
    }

    public class HexShapeGenerator : IHexGenerator
    {
        private readonly HexShapeType type;
        private readonly int radius;
        private readonly int corridorThickness;

        public HexShapeGenerator(HexShapeType type, int radius, int corridorThickness)
        {
            this.type = type;
            this.radius = Mathf.Max(0, radius);
            this.corridorThickness = Mathf.Max(1, corridorThickness);
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

                case HexShapeType.TwoRoomsWithCorridor:
                    foreach (var hex in TwoRoomsWithCorridor(start, radius, corridorThickness)) set.Add(hex);
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown generation type: {type}");
            }

            return set;
        }

        private static IEnumerable<HexCoord> Disk(HexCoord center, int radius)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius); dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    yield return new HexCoord(center.Q + dq, center.R + dr);
                }
            }
        }

        private static IEnumerable<HexCoord> Ring(HexCoord center, int radius)
        {
            if (radius <= 0)
            {
                yield return center;
                yield break;
            }
            
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius);
                    dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    var hex = new HexCoord(center.Q + dq, center.R + dr);

                    if (center.Distance(hex) == radius)
                        yield return hex;
                } 
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
