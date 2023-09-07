using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class PropertyModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Space]
        [Tooltip("Target object that will be modified in the imported prefab/scene if the conditions above are met")]
        public GameObjectOrComponent Target;

        protected override void Apply(bool filtersMatch)
        {
            Object target = Target.Object;
            if (filtersMatch && target != null)
            {
                UnityEditor.Presets.Preset preset = EmbeddedPresetHolder.Instance.GetPreset(this, target);
                if (preset != null)
                {
                    preset.ApplyTo(target);
                }
            }
        }
#endif
    }
}
