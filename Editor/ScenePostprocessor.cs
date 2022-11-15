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
#if UNITY_2020_1_OR_NEWER
            FilteredObjectModifier[] foundComponents = Object.FindObjectsOfType<FilteredObjectModifier>(true);
#else
            FilteredObjectModifier[] foundComponents = Object.FindObjectsOfType<FilteredObjectModifier>();
#endif
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            string[] scriptingDefineSymbols = ScriptingDefineSymbolsDependency.ScriptingDefineSymbols;
            foreach (FilteredObjectModifier objectModifier in foundComponents)
            {
                objectModifier.Apply(isEditor, isDevelopment, selectedBuildTarget, scriptingDefineSymbols);
                Object.DestroyImmediate(objectModifier, true);
            }
        }
    }
}
