using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalObjects : FilteredObjectModifier
    {
#if UNITY_EDITOR
        [Space]
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
