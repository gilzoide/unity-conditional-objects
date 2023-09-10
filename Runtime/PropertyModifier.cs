using Gilzoide.ConditionalObjects.Internal;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class PropertyModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Properties that should be modified in the imported prefab/scene if the conditions above are met")]
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
