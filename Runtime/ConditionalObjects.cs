using Gilzoide.ConditionalObjects.Filters;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalObjects : FilteredObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        public Object[] TargetObjects;

        public override void Apply()
        {
            foreach (Object obj in TargetObjects)
            {
                DestroyImmediate(obj, true);
            }
        }

        protected override IncludeMode GetApplyIncludeMode()
        {
            return IncludeMode.Exclude;
        }
#endif
    }
}
