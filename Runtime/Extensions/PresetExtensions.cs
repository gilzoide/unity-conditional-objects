#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Presets;

namespace Gilzoide.ConditionalObjects
{
    public static class PresetExtensions
    {
        [ThreadStatic] private static HashSet<string> _propertyNames = new HashSet<string>();

        public static void ExcludeAllProperties(this Preset preset)
        {
            preset.GetIncludedProperties(_propertyNames);
            preset.excludedProperties = _propertyNames.ToArray();
        }

        public static void GetIncludedProperties(this Preset preset, HashSet<string> set)
        {
            set.Clear();
            if (preset.PropertyModifications == null)
            {
                return;
            }
            set.UnionWith(preset.PropertyModifications.Select(prop => prop.propertyPath));
            set.UnionWith(preset.PropertyModifications.Select(prop => prop.propertyPath.Split('.', 2)[0]));
        }
    }
}
#endif