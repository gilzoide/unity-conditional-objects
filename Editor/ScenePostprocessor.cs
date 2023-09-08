using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class ScenePostprocessor : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
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

            var referencedPresets = new HashSet<Preset>();

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            string[] scriptingDefineSymbols = ScriptingDefineSymbolsDependency.ScriptingDefineSymbols;
            foreach (AImportTimeObjectModifier objectModifier in foundComponents)
            {
                if (objectModifier is PropertyModifier propertyModifier && propertyModifier._preset != null)
                {
                    referencedPresets.Add(propertyModifier._preset);
                }
                objectModifier.Apply(isEditor, isDevelopment, selectedBuildTarget, scriptingDefineSymbols);
                Object.DestroyImmediate(objectModifier, true);
            }

            EmbeddedPresetHolder.Instance.DeleteOrphanPresets(scene.path, referencedPresets);
        }
    }
}
