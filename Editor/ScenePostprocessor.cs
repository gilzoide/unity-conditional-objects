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
            FilteredObjectModifier[] foundComponents = Object.FindObjectsOfType<FilteredObjectModifier>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            foreach (FilteredObjectModifier objectModifier in foundComponents)
            {
                if (objectModifier.ShouldApply(isEditor, isDevelopment, selectedBuildTarget))
                {
                    objectModifier.Apply();
                }
                Object.DestroyImmediate((Object) objectModifier, true);
            }
        }
    }
}
