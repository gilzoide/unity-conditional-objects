using Gilzoide.ConditionalObjects.Filters;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalProperties : FilteredObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        public PropertyVariant[] Properties;

        public override void Apply()
        {
            foreach (PropertyVariant variant in Properties)
            {
                variant.Apply();
            }
        }

        protected override IncludeMode GetApplyIncludeMode()
        {
            return IncludeMode.Include;
        }
#endif
    }
}
