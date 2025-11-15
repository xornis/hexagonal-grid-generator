using System.Collections.Generic;
using UnityEngine;

namespace HexDungeon
{
    public enum HexShapeType
    {
        Disk, Ring, Experimental
    }

    public static class HexGridShape
    {
        public static IEnumerable<HexCoord> Generate(HexShapeType type, HexCoord center, int radius)
        {
            return type switch
            {
                HexShapeType.Disk => Disk(center, radius),
                HexShapeType.Ring => Ring(center, radius),
                HexShapeType.Experimental => Experimental(center, radius),
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

        public static IEnumerable<HexCoord> Experimental(HexCoord center, int radius)
        {
            for (int dq = -radius; dq <= radius; dq++)
            {
                for (int dr = Mathf.Max(-radius, -dq - radius);
                    dr <= Mathf.Min(radius, -dq + radius); dr++)
                {
                    var hex = new HexCoord(center.Q + dq, center.R + dr);
                    yield return hex;
                }
            }
        }
    }
}
