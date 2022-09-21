using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class ScenePostprocessor
    {
        [PostProcessScene]
        static void OnPostprocessScene()
        {
            ConditionalObjects[] foundComponents = Object.FindObjectsOfType<ConditionalObjects>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            foreach (ConditionalObjects conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate(conditionalObjects, true);
            }
        }
    }
}
