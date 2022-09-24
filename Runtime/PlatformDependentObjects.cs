using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class PlatformDependentObjects : MonoBehaviour, IPlatformDependentObjectModifier
    {
#if UNITY_EDITOR
        public PlatformFilter Condition;
        public Object[] TargetObjects;

        public void ApplyForTarget(BuildTarget buildTarget)
        {
            if (Condition.IsPlatformExcluded(buildTarget))
            {
                foreach (Object obj in TargetObjects)
                {
                    DestroyImmediate(obj, true);
                }
            }
        }
#endif
    }
}
