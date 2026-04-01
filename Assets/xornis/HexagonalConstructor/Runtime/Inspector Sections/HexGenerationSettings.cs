using UnityEngine;

namespace HexagonalConstructor
{
    public class HexGenerationSettings : MonoBehaviour
    {
        [SerializeField, Tooltip("Axial coordinates (Q, R) of the starting hex. \nX = Q \nY = R")] private Vector2Int startAxial;
        [SerializeField] private GenerationMode generationMode;
        [SerializeField, SerializeReference] private ShapeGenerator shapeGenerator;
        [SerializeField, SerializeReference] private RandomizedGenerator randomizedGenerator;

        public HexCoord StartHex => new HexCoord(startAxial.x, startAxial.y);
        public SerializableHexGenerator CurrentGenerator =>
            generationMode == GenerationMode.Randomized
            ? randomizedGenerator
            : shapeGenerator;
    }
}
