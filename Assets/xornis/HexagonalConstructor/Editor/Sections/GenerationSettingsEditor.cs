#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(HexGenerationSettings))]
    public class GenerationSettingsEditor : UnityEditor.Editor, IEditorSection
    {
        private HexGenerationSettings generationSettings;

        private bool foldout = true;

        private void OnEnable()
        {
            generationSettings = (HexGenerationSettings)target;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        public void Draw()
        {
            EditorHelper.DrawFoldout(ref foldout, generationSettings.GetType().Name, () =>
            {
                DrawStartAxial();
                DrawGenerationMode();
            });
        }

        private void DrawStartAxial()
        {
            EditorHelper.DrawProperties(serializedObject, "startAxial");
        }

        private void DrawGenerationMode()
        {
            var generationModeProp = serializedObject.FindProperty("generationMode");
            bool isGenerationRandomized = generationModeProp.enumValueIndex == (int)GenerationMode.Randomized;
            var generatorProp = isGenerationRandomized ? "randomizedGenerator" : "shapeGenerator";

            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperties(serializedObject, generationModeProp.propertyPath, generatorProp);
            });

            //DrawButton("Randomize Seed", EditorRandomizeSeedInternal);
        }
    }
}
#endif