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
        public static IEnumerable<HexCoord> Generate(HexShapeType type, HexCoord center, int radius)
        {
            return type switch
            {
                HexShapeType.Disk => Disk(center, radius),
                HexShapeType.Ring => Ring(center, radius),
                HexShapeType.TwoRoomsWithCorridor => TwoRoomsWithCorridor(),
                _ => null
            };
        }


        public static IEnumerable<HexCoord> Disk(HexCoord center, int radius)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius); dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    yield return new HexCoord(center.Q + dq, center.R + dr);
                }
            }
        }

        public static IEnumerable<HexCoord> Ring(HexCoord center, int radius)
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

        public static IEnumerable<HexCoord> Corridor(HexCoord start, HexCoord end)
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

        public static IEnumerable<HexCoord> TwoRoomsWithCorridor()
        {
            HexCoord roomACenter = new HexCoord(0, 0);
            HexCoord roomBCenter = new HexCoord(5, -10);
            HexCoord roomCCenter = new HexCoord(-10, -20);

            foreach (var hex in Disk(roomACenter, 3)) yield return hex;
            foreach (var hex in Disk(roomBCenter, 2)) yield return hex;
            foreach (var hex in Disk(roomCCenter, 2)) yield return hex;
            foreach (var hex in Corridor(roomACenter, roomBCenter)) yield return hex;
            foreach (var hex in Corridor(roomBCenter, roomCCenter)) yield return hex;
            foreach (var hex in Corridor(roomCCenter, roomACenter)) yield return hex;
        }
    }
}
