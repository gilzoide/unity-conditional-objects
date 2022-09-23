using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalProperties : MonoBehaviour, IObjectModifierByPlatform
    {
#if UNITY_EDITOR
        public PlatformCondition Platforms;
        public PropertyVariant[] Properties;

        public void ApplyForTarget(BuildTarget buildTarget)
        {
            if (Platforms.IsPlatformIncluded(buildTarget))
            {
                foreach (PropertyVariant variant in Properties)
                {
                    variant.Apply();
                }
            }
        }
#endif
    }
}
