using UnityEngine;

namespace HexDungeon
{
    public abstract class RandomizedGenerator : SerializableHexGenerator
    {
        [SerializeField] protected int hexCount = 100;
        [SerializeField] protected bool useSeed;
        [SerializeField, Tooltip("Works only when useSeed is true")] protected int seed;
    }
}
