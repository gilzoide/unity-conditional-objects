using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalObjects : MonoBehaviour, IObjectModifierByPlatform
    {
#if UNITY_EDITOR
        public PlatformCondition Condition;
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
