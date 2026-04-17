#if UNITY_EDITOR
using UnityEditor;

namespace HexagonalConstructor.Editor
{
    public abstract class SettingsEditorBase : UnityEditor.Editor, IEditorSection
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(
                $"{target.GetType().Name} is managed via GridGenerator Inspector.",
                MessageType.Info
            );
        }

        public abstract void Draw();
    }
}
#endif