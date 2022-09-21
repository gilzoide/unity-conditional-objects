using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class PrefabPostprocessor : AssetPostprocessor
    {
        void OnPostprocessPrefab(GameObject gameObject)
        {
            ConditionalObjects[] foundComponents = gameObject.GetComponentsInChildren<ConditionalObjects>();
            if (foundComponents == null || foundComponents.Length == 0)
            {
                return;
            }

            BuildTarget selectedBuildTarget = context.selectedBuildTarget;
            foreach (ConditionalObjects conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate(conditionalObjects, true);
            }
        }
    }
}
