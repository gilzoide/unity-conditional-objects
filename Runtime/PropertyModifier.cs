using UnityEditor.Presets;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class PropertyModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Target object that will be modified in the imported prefab/scene if the conditions above are met")]
        public GameObjectOrComponent Target;
        [SerializeField, ReadOnlyProperty] internal Preset _preset;

        protected override void Apply(bool filtersMatch)
        {
            Object target = Target.Object;
            if (filtersMatch && target != null && _preset != null)
            {
                _preset.ApplyTo(target);
            }
        }
#endif
    }
}
