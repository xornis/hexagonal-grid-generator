using UnityEngine;

namespace HexDungeon
{
    public class HexGridSettings : MonoBehaviour
    {
        [SerializeField, Tooltip("Note: Hex sprite must be oriented correctly. Generator does NOT auto-rotate sprites.")] private GameObject hexPrefab;
        [SerializeField] private float hexScale = 1f;
        [SerializeField] private HexOrientation hexOrientation = HexOrientation.FlatTop;
        [SerializeField] private float hexRadius = 1f;

        public GameObject HexPrefab => hexPrefab;
        public float HexScale => hexScale;
        public HexLayout HexLayout => new HexLayout(hexOrientation, hexRadius);
    }
}
