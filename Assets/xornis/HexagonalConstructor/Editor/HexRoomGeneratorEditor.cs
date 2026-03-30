#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon
{
    [CustomEditor(typeof(HexRoomGenerator))]
    public class HexRoomGeneratorEditor : UnityEditor.Editor
    {
        private HexRoomGenerator mainGen;

        private void OnEnable()
        {
            mainGen = (HexRoomGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject


            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
        }
    }
}
#endif
