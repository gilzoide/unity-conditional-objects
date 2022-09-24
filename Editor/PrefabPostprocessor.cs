using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            IPlatformDependentObjectModifier[] foundComponents = gameObject.GetComponentsInChildren<IPlatformDependentObjectModifier>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            BuildTarget selectedBuildTarget = context.selectedBuildTarget;
            foreach (IPlatformDependentObjectModifier conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }
        }
    }
}
