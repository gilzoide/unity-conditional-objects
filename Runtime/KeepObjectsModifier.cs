using Gilzoide.ConditionalObjects.Internal;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class KeepObjectsModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Objects that should be kept in the imported prefab/scene only if the conditions above are met")]
        [GameObjectOrComponent] public Object[] TargetObjects;

        protected override void Apply(bool filtersMatch)
        {
            if (!filtersMatch)
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
