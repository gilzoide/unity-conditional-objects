using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class DevelopmentDependentProperties : MonoBehaviour, IDevelopmentDependentObjectModifier
    {
#if UNITY_EDITOR
        [Tooltip("Whether properties will be applied on development or release builds")]
        public bool Development = true;
        public PropertyVariant[] Properties;

        public void Apply(bool isDevelopment)
        {
            if (isDevelopment == Development)
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
