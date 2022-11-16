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
            ImportTimeObjectModifier[] foundComponents = Object.FindObjectsOfType<ImportTimeObjectModifier>(true);
#else
            ImportTimeObjectModifier[] foundComponents = Object.FindObjectsOfType<ImportTimeObjectModifier>();
#endif
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            string[] scriptingDefineSymbols = ScriptingDefineSymbolsDependency.ScriptingDefineSymbols;
            foreach (ImportTimeObjectModifier objectModifier in foundComponents)
            {
                objectModifier.Apply(isEditor, isDevelopment, selectedBuildTarget, scriptingDefineSymbols);
                Object.DestroyImmediate(objectModifier, true);
            }
        }
    }
}
