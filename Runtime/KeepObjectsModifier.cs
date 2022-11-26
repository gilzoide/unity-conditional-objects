using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class KeepObjectsModifier : ImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Objects that should be kept in the imported prefab/scene only if the conditions above are met")]
        public GameObjectOrComponent[] TargetObjects;

        protected override void Apply(bool filtersMatch)
        {
            if (!filtersMatch)
            {
                foreach (GameObjectOrComponent target in TargetObjects)
                {
                    DestroyImmediate(target.Object, true);
                }
            }
        }
#endif
    }
}
