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

            EditorGUI.BeginChangeCheck();

            EditorHelper.DrawFoldout(ref foldout, Settings.GetType().Name, () =>
            {
                DrawStartAxial();
                DrawGenerationMode();
            });

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                PreviewSettings.InvokeForceRebuild();
            }
            else serializedObject.ApplyModifiedProperties();
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