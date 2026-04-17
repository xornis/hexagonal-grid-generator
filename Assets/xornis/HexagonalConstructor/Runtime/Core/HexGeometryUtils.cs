using UnityEngine;

namespace HexagonalConstructor
{
    internal static class HexGeometryUtils
    {
        internal static Vector3 AxialToCube(HexCoord hex)
        {
            int x = hex.Q;
            int z = hex.R;
            int y = -x - z;
            return new Vector3(x, y, z);
        }

        internal static HexCoord CubeToAxial(Vector3 vector)
        {
            int q = Mathf.RoundToInt(vector.x);
            int r = Mathf.RoundToInt(vector.z);
            return new HexCoord(q, r);
        }

        internal static Vector3 CubeRound(Vector3 vector)
        {
            int rx = Mathf.RoundToInt(vector.x);
            int ry = Mathf.RoundToInt(vector.y);
            int rz = Mathf.RoundToInt(vector.z);

            float dx = Mathf.Abs(rx - vector.x);
            float dy = Mathf.Abs(ry - vector.y);
            float dz = Mathf.Abs(rz - vector.z);

            if (dx > dy && dx > dz)
                rx = -ry - rz;
            else if (dy > dz)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            return new Vector3(rx, ry, rz);
        }
    }
}
