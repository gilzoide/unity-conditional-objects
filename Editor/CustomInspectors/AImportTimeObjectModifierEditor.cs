#if !UNITY_2020_2_OR_NEWER
using UnityEditor;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomEditor(typeof(AImportTimeObjectModifier), true)]
    public class AImportTimeObjectModifierEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (target.IsPartOfPrefab())
            {
                EditorGUILayout.HelpBox($"{target.GetType().Name} in Prefab will only work when prefab is instanced in scenes. Use Unity 2020.2+ for full prefab support.", MessageType.Warning);
            }

            DrawDefaultInspector();
        }
    }
}
#endif