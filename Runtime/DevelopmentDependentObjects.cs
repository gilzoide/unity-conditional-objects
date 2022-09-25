using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class DevelopmentDependentObjects : MonoBehaviour, IDevelopmentDependentObjectModifier
    {
#if UNITY_EDITOR
        [Tooltip("Whether objects will be kept on development or release builds")]
        public bool Development = true;
        public Object[] TargetObjects;

        public void Apply(bool isDevelopment)
        {
            if (isDevelopment != Development)
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
