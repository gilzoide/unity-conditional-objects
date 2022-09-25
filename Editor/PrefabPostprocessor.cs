#if UNITY_2020_2_OR_NEWER
using Gilzoide.ConditionalObjects.Filters;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            FilteredObjectModifier[] foundComponents = gameObject.GetComponentsInChildren<FilteredObjectModifier>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            bool isEditor = EditorDependency.IsEditor;
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            foreach (FilteredObjectModifier objectModifier in foundComponents)
            {
                if (objectModifier.EditorFilter != EditorFilter.None)
                {
                    context.DependsOnCustomDependency(EditorDependency.DependencyName);
                }
                if (objectModifier.DevelopmentFilter != DevelopmentFilter.None)
                {
                    context.DependsOnCustomDependency(DevelopmentDependency.DependencyName);
                }
                BuildTarget selectedBuildTarget = objectModifier.PlatformFilter.IsEmpty
                    ? BuildTarget.NoTarget
                    : context.selectedBuildTarget;
                if (objectModifier.ShouldApply(isEditor, isDevelopment, selectedBuildTarget))
                {
                    objectModifier.Apply();
                }
                Object.DestroyImmediate((Object) objectModifier, true);
            }
        }
    }
}
#endif