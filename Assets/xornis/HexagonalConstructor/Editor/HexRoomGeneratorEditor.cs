#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexagonalConstructor
{
    public interface IEditorSection
    {
        void Draw();
    }

    [CustomEditor(typeof(HexRoomGenerator))]
    public class HexRoomGeneratorEditor : UnityEditor.Editor
    {
        private HexRoomGenerator mainGen;
        private UnityEditor.Editor[] sectionEditors;

        private void OnEnable()
        {
            mainGen = (HexRoomGenerator)target;
            InitializeEditors();
        }

        private void OnDisable()
        {
            DestroyEditors();
        }

        private void InitializeEditors()
        {
            var sections = new Component[]
            {
                mainGen.GetComponent<HexGridSettings>(),
                mainGen.GetComponent<HexGenerationSettings>(),
                mainGen.GetComponent<HexPreviewSettings>(),
                mainGen.GetComponent<HexDebugSettings>()
            };

            sectionEditors = sections
                .Where(s => s != null)
                .Select(s => CreateEditor(s))
                .ToArray();
        }

        private void DestroyEditors()
        {
            if (sectionEditors == null) return;

            foreach (var editor in sectionEditors)
            {
                if (editor != null)
                    DestroyImmediate(editor);
            }

            sectionEditors = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); // Loading MonoBehaviour fields to SerializedObject

            if (sectionEditors != null)
            {
                foreach (var editor in sectionEditors)
                {
                    if (editor != null)
                        editor.OnInspectorGUI();
                }
            }

            serializedObject.ApplyModifiedProperties(); // Saving changes made in the Inspector
        }
    }
}
#endif
