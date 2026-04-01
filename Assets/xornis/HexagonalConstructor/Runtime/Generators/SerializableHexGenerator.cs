using System.Collections.Generic;

namespace HexagonalConstructor
{
    public interface IHexGenerator
    {
        IEnumerable<HexCoord> Generate(HexCoord start);
    }

    [System.Serializable]
    public abstract class SerializableHexGenerator : IHexGenerator
    {
        public abstract IEnumerable<HexCoord> Generate(HexCoord start);
    }
}
