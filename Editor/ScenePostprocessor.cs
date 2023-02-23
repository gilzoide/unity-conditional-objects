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
            AImportTimeObjectModifier[] foundComponents = Object.FindObjectsOfType<AImportTimeObjectModifier>(true);
#else
            AImportTimeObjectModifier[] foundComponents = Object.FindObjectsOfType<AImportTimeObjectModifier>();
#endif
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            string[] scriptingDefineSymbols = ScriptingDefineSymbolsDependency.ScriptingDefineSymbols;
            foreach (AImportTimeObjectModifier objectModifier in foundComponents)
            {
                objectModifier.Apply(isEditor, isDevelopment, selectedBuildTarget, scriptingDefineSymbols);
                Object.DestroyImmediate(objectModifier, true);
            }
        }
    }
}
