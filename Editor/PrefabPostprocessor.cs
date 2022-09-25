using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            ProcessPlatformDependent(gameObject);
            ProcessDevelopmentDependent(gameObject);
        }

        void ProcessPlatformDependent(GameObject gameObject)
        {
            IPlatformDependentObjectModifier[] foundComponents = gameObject.GetComponentsInChildren<IPlatformDependentObjectModifier>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            BuildTarget selectedBuildTarget = context.selectedBuildTarget;
            foreach (IPlatformDependentObjectModifier objectModifier in foundComponents)
            {
                objectModifier.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) objectModifier, true);
            }
        }

        void ProcessDevelopmentDependent(GameObject gameObject)
        {
            IDevelopmentDependentObjectModifier[] foundComponents = gameObject.GetComponentsInChildren<IDevelopmentDependentObjectModifier>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            context.DependsOnCustomDependency(DevelopmentDependency.DependencyName);
            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            foreach (IDevelopmentDependentObjectModifier objectModifier in foundComponents)
            {
                objectModifier.Apply(isDevelopment);
                Object.DestroyImmediate((Object) objectModifier, true);
            }
        }
    }
}
#endif