using System.Collections.Generic;

namespace HexDungeon
{
    [System.Serializable]
    public abstract class SerializableHexGenerator : IHexGenerator
    {
        public abstract IEnumerable<HexCoord> Generate(HexCoord start);
    }
}
