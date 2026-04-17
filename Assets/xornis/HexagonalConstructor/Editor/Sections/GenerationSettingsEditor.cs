#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    [CustomEditor(typeof(GenerationSettings))]
    public class GenerationSettingsEditor : SettingsEditorBase
    {
        private bool foldout = true;

        private GenerationSettings Settings => (GenerationSettings)target;

        public override void Draw()
        {
            serializedObject.Update();

            EditorHelper.DrawFoldout(ref foldout, Settings.GetType().Name, () =>
            {
                DrawStartAxial();
                DrawGenerationMode();
            });

            serializedObject.ApplyModifiedProperties();
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