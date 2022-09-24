using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(PlatformFilter))]
    public class PlatformFilterPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty platforms = property.FindPropertyRelative(nameof(PlatformFilter.Platforms));
            if (platforms.arraySize > 0)
            {
                SerializedProperty filter = property.FindPropertyRelative(nameof(PlatformFilter.Filter));
                PlatformFilter.FilterType  filterType = (PlatformFilter.FilterType) filter.enumValueIndex;
                label.text = $"{(filterType == PlatformFilter.FilterType.Include ? "Only on" : "Unless on")}: {string.Join(", ", platforms.IterateEnumArrayStrings())}";
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
