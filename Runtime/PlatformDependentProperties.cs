using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class PlatformDependentProperties : MonoBehaviour, IPlatformDependentObjectModifier
    {
#if UNITY_EDITOR
        public PlatformFilter Platforms;
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
