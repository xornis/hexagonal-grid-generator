using UnityEngine;

namespace HexagonalConstructor
{
    public enum HexOrientation
    {
        FlatTop,
        PointyTop
    }

    public class HexLayout
    {
        public HexOrientation Orientation { get; }
        public float Size { get; }

        public HexLayout(HexOrientation orientation, float size)
        {
            Orientation = orientation;
            Size = Mathf.Max(0.0001f, size);
        }

        public Vector3 HexToWorld(HexCoord hex) =>
            Orientation == HexOrientation.FlatTop
            ? FlatTopHexToWorld(hex)
            : PointyTopHexToWorld(hex);

        private Vector3 FlatTopHexToWorld(HexCoord hex)
        {
            float size = Size;

            float w = size * 2f;
            float h = Mathf.Sqrt(3f) * size;

            float x = (3f * size / 2f) * hex.Q;
            float y = (h * (hex.R + hex.Q * 0.5f));

            return new Vector3(x, y, 0f);
        }

        private Vector3 PointyTopHexToWorld(HexCoord hex)
        {
            float size = Size;

            float w = Mathf.Sqrt(3f) * size;
            float h = size * 2f;

            float x = w * (hex.Q + hex.R * 0.5f);
            float y = (3f * size / 2f) * hex.R;

            return new Vector3(x, y, 0f);
        }
    }
}
