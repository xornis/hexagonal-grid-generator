using UnityEngine;

namespace HexDungeon
{
    [System.Serializable]
    public readonly struct HexCoord
    {
        public int Q { get; }
        public int R { get; }
        public int S => -Q - R;

        public HexCoord(int q, int r)
        {
            Q = q;
            R = r;
        }
        
        public override string ToString() => $"HexCoord({Q}, {R}, {S})";
    }
}
