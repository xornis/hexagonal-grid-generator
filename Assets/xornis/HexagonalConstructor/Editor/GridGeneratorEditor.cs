#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexagonalConstructor.Editor
{
    public interface IEditorSection
    {
        void Draw();
    }

    [CustomEditor(typeof(GridGenerator))]
    public class GridGeneratorEditor : UnityEditor.Editor
    {
        private GridGenerator mainGen;
        private UnityEditor.Editor[] sectionEditors;

        private void OnEnable()
        {
            mainGen = (GridGenerator)target;
            InitializeEditors();
        }

        private void OnDisable()
        {
            DestroyEditors();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update(); 

            if (!ValidateRequiredComponents())
            {
                EditorGUILayout.HelpBox("GridGenerator requires: GeneratorContext, GridSettings, GenerationSettings", MessageType.Error);

                if (GUILayout.Button("Add Required Components"))
                {
                    AddRequiredComponents();
                    InitializeEditors();
                }

                serializedObject.ApplyModifiedProperties();
                return;
            }

            DrawSections();

            if (!ValidateOptionalComponents())
            {
                EditorGUILayout.HelpBox("Optional components: PreviewSettings, DebugSettings", MessageType.None);
                if (GUILayout.Button("Add Optional Components"))
                    AddOptionalComponents();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeEditors()
        {
            var sections = new Component[]
            {
                mainGen.GetComponent<GridSettings>(),
                mainGen.GetComponent<GenerationSettings>(),
                mainGen.GetComponent<PreviewSettings>(),
                mainGen.GetComponent<DebugSettings>()
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

        private void DrawSections()
        {
            if (sectionEditors == null) return;

            foreach (var editor in sectionEditors)
            {
                if (editor != null && editor is IEditorSection section)
                    section.Draw();
            }
        }

        private bool ValidateRequiredComponents()
        {
            return mainGen.GetComponent<GeneratorContext>() != null
                && mainGen.GetComponent<GridSettings>() != null 
                && mainGen.GetComponent<GenerationSettings>() != null;
        }

        private bool ValidateOptionalComponents()
        {
            return mainGen.GetComponent<PreviewSettings>() != null 
                && mainGen.GetComponent<DebugSettings>() != null;
        }

        private void AddRequiredComponents()
        {
            AddComponent<GeneratorContext>();
            AddComponent<GridSettings>();
            AddComponent<GenerationSettings>();
        }
        
        private void AddOptionalComponents()
        {
            AddComponent<PreviewSettings>();
            AddComponent<DebugSettings>();
        }

        private void AddComponent<T>() where T : Component
        {
            var component = mainGen.GetComponent<T>();

            if (component == null)
                mainGen.gameObject.AddComponent<T>();
        }
    }
}
#endif
