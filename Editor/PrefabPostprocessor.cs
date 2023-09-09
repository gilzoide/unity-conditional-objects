#if UNITY_2020_2_OR_NEWER
using System.Collections.Generic;
using Gilzoide.ConditionalObjects.Filters;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            AImportTimeObjectModifier[] foundComponents = gameObject.GetComponentsInChildren<AImportTimeObjectModifier>(true);
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            var referencedPresets = new HashSet<Preset>();

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            string[] scriptingDefineSymbols = ScriptingDefineSymbolsDependency.ScriptingDefineSymbols;
            foreach (AImportTimeObjectModifier objectModifier in foundComponents)
            {
                if (objectModifier.EditorFilter != EditorFilter.None)
                {
                    context.DependsOnCustomDependency(EditorDependency.DependencyName);
                }
                if (objectModifier.DevelopmentFilter != DevelopmentFilter.None)
                {
                    context.DependsOnCustomDependency(DevelopmentDependency.DependencyName);
                }
                if (!objectModifier.ScriptingDefineConstraints.IsEmpty)
                {
                    context.DependsOnCustomDependency(ScriptingDefineSymbolsDependency.DependencyName);
                }
                if (objectModifier is PropertyModifier propertyModifier && propertyModifier._preset != null)
                {
                    context.DependsOnSourceAsset(EmbeddedPresetHolder.InstanceAssetPath);
                    referencedPresets.Add(propertyModifier._preset);
                }
                BuildTarget selectedBuildTarget = objectModifier.PlatformFilter.IsEmpty
                    ? BuildTarget.NoTarget
                    : context.selectedBuildTarget;
                objectModifier.Apply(isEditor, isDevelopment, selectedBuildTarget, scriptingDefineSymbols);
                Object.DestroyImmediate(objectModifier, true);
            }

            EmbeddedPresetHolder.Instance.DeleteOrphanPresets(context.assetPath, referencedPresets);
        }
    }
}
#endif