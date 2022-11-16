using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class DeleteObjectsModifier : ImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Objects that should be deleted from the imported prefab/scene if the conditions above are met")]
        public Object[] TargetObjects;

        protected override void Apply(bool filtersMatch)
        {
            if (filtersMatch)
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
