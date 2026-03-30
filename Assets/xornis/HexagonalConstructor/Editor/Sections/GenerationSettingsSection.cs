#if UNITY_EDITOR
using UnityEditor;

namespace HexDungeon.Editor
{
    public class GenerationSettingsSection
    {
        private readonly SerializedObject serializedObject;

        private bool foldout = true;

        public GenerationSettingsSection(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
        }

        public void Draw()
        {
            foldout = EditorGUILayout.Foldout(foldout, "Generation Settings", true, EditorStyles.foldoutHeader);
            if (!foldout) return;

            EditorHelper.Indent(() =>
            {
                DrawStartAxial();
                EditorGUILayout.Space(6);
                DrawGenerationMode();
            });
        }

        private void DrawStartAxial()
        {
            EditorHelper.DrawProperty("startAxial", serializedObject);
        }

        private void DrawGenerationMode()
        {
            var generationModeProp = serializedObject.FindProperty("generationMode");
            bool isGenerationRandomized = generationModeProp.enumValueIndex == (int)GenerationMode.Randomized;
            var generatorProp = isGenerationRandomized ? "randomizedGenerator" : "shapeGenerator";

            EditorHelper.Indent(() =>
            {
                EditorHelper.DrawProperty("generationMode", serializedObject);
                EditorHelper.DrawProperty(generatorProp, serializedObject);
            });

            //DrawButton("Randomize Seed", EditorRandomizeSeedInternal);
        }
    }
}
#endif