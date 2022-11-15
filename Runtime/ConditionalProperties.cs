using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalProperties : FilteredObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        public PropertyVariant[] Properties;

        protected override void Apply(bool filtersMatch)
        {
            if (filtersMatch)
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
