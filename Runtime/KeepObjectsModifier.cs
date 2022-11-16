using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class KeepObjectsModifier : ImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Objects that should be kept in the imported prefab/scene only if the conditions above are met")]
        public Object[] TargetObjects;

        protected override void Apply(bool filtersMatch)
        {
            if (!filtersMatch)
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
