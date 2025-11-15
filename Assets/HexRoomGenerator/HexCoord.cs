using UnityEngine;

namespace HexDungeon
{
    [System.Serializable]
    public readonly struct HexCoord
    {
        public int Q { get; }
        public int R { get; }
        public int S => -Q - R;

        public static HexCoord Zero { get; } = new HexCoord(0, 0);

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }
        
        public override string ToString() => $"HexCoord({Q}, {R}, {S})";
    }

    public static class HexCoordExtensions
    {
        private static readonly (int dq, int dr)[] directions =
        {
            (1, 0), // NorthEast
            (1, -1), // East
            (0, -1), // SouthEast
            (-1, 0), // SouthWest
            (-1, 1), // West
            (0, 1), // NorthWest
        };

        public static HexCoord Neighbor(this HexCoord hex, HexDirection dir)
        {
            var (dq, dr) = directions[(int)dir];
            return new HexCoord(hex.Q + dq, hex.R + dr);
        }

        public static HexCoord Add(this HexCoord a, HexCoord b) => new HexCoord(a.Q + b.Q, a.R + b.R);

        public static HexCoord Scale (this HexCoord h, int k) => new HexCoord(h.Q * k, h.R * k);

        public static int Distance(this HexCoord a, HexCoord b)
        {
            int dq = a.Q - b.Q;
            int dr = a.R - b.R;
            return (Mathf.Abs(dq) + Mathf.Abs(dq + dr) + Mathf.Abs(dr)) / 2;
        }
    }
}
