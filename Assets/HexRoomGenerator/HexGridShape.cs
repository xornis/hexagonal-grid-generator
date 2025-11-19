using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexShapeType
    {
        Disk, Ring, TwoRoomsWithCorridor,
    }

    public static class HexGridShape
    {
        public static IEnumerable<HexCoord> Generate(HexShapeType type, HexCoord center, int radius, int corridorThickness)
        {
            switch (type)
            {
                case HexShapeType.Disk: return Disk(center, radius);
                case HexShapeType.Ring: return Ring(center, radius);
                case HexShapeType.TwoRoomsWithCorridor: return TwoRoomsWithCorridor(corridorThickness);
                default:
                    Debug.LogWarning($"HexDungeon: unsupported shape generation type {type}");
                    return Empty();
            }
        }

        private static IEnumerable<HexCoord> Empty()
        {
            yield break;
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
                foreach (var thickHex in ExpandThickness(hex, thickness))
                    yield return thickHex;
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

        private static IEnumerable<HexCoord> ExpandThickness(HexCoord center, int thickness)
        {
            yield return center;

            if (thickness <= 1) yield break;

            for (int ring = 1; ring < thickness; ring++)
                foreach (var hex in Ring(center, ring))
                    yield return hex;
        }

        private static IEnumerable<HexCoord> TwoRoomsWithCorridor(int thickness)
        {
            HexCoord roomACenter = new HexCoord(0, 0);
            HexCoord roomBCenter = new HexCoord(25, -10);

            foreach (var hex in Disk(roomACenter, 3)) yield return hex;
            foreach (var hex in Disk(roomBCenter, 2)) yield return hex;
            foreach (var hex in Corridor(roomACenter, roomBCenter, thickness)) yield return hex;
        }
    }
}
