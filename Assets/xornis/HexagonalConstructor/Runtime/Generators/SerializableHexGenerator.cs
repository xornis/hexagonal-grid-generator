using System.Collections.Generic;

namespace HexDungeon
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
