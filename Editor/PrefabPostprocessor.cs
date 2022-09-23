using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            IObjectModifierByPlatform[] foundComponents = gameObject.GetComponentsInChildren<IObjectModifierByPlatform>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            BuildTarget selectedBuildTarget = context.selectedBuildTarget;
            foreach (IObjectModifierByPlatform conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }
        }
    }
}
