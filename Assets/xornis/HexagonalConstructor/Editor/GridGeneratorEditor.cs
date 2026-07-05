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
        private bool hideComponents;

        private GridGenerator mainGen;
        private UnityEditor.Editor[] sectionEditors;

        private void OnEnable()
        {
            mainGen = (GridGenerator)target;
            InitializeEditors();

            hideComponents = SessionState.GetBool("HideComponents_" + mainGen.gameObject.GetInstanceID(), false);

            ToggleComponentsVisibility(hideComponents, forceRefresh: false);
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

            var genSettings = mainGen.GetComponent<GenerationSettings>();
            if (genSettings != null && genSettings.CurrentGenerator == null)
                EditorGUILayout.HelpBox("Please select a Generator Type in Generation Settings before attempting to build the grid.", MessageType.Warning);

            ChangeComponentsVisibility();

            DrawSections();

            if (!ValidateOptionalComponents())
            {
                EditorGUILayout.HelpBox("Optional components: PreviewSettings, DebugSettings", MessageType.None);
                if (GUILayout.Button("Add Optional Components"))
                {
                    AddOptionalComponents();
                    InitializeEditors();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeEditors()
        {
            DestroyEditors();

            var components = new Component[]
            {
                mainGen.GetComponent<GridSettings>(),
                mainGen.GetComponent<GenerationSettings>(),
                mainGen.GetComponent<PreviewSettings>(),
                mainGen.GetComponent<DebugSettings>()
            };

            sectionEditors = components
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

        private void ChangeComponentsVisibility()
        {
            EditorGUI.BeginChangeCheck();
            hideComponents = GUILayout.Toggle(hideComponents, "Hide Components");
            if (EditorGUI.EndChangeCheck())
                ToggleComponentsVisibility(hideComponents, forceRefresh: true);
        }

        private void ToggleComponentsVisibility(bool state, bool forceRefresh)
        {
            var components = new Component[]
            {
                mainGen.GetComponent<GridSettings>(),
                mainGen.GetComponent<GenerationSettings>(),
                mainGen.GetComponent<PreviewSettings>(),
                mainGen.GetComponent<DebugSettings>(),
                mainGen.GetComponent<GeneratorContext>()
            };

            foreach (var c in components)
            {
                if (c == null) continue;
                c.hideFlags = state ? HideFlags.HideInInspector : HideFlags.None;
                SessionState.SetBool("HideComponents_" + mainGen.gameObject.GetInstanceID(), state);
            }

            EditorUtility.SetDirty(mainGen.gameObject);

            if (forceRefresh)
            {
                ActiveEditorTracker.sharedTracker.ForceRebuild();
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
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
