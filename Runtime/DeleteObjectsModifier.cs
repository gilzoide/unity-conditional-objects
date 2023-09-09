using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class DeleteObjectsModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Objects that should be deleted from the imported prefab/scene if the conditions above are met")]
        [GameObjectOrComponent] public Object[] TargetObjects;

        protected override void Apply(bool filtersMatch)
        {
            if (filtersMatch)
            {
                foreach (Object target in TargetObjects)
                {
                    if (target is Component || target is GameObject)
                    {
                        DestroyImmediate(target, true);
                    }
                }
            }
        }
#endif
    }
}
