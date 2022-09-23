using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(PlatformCondition))]
    public class PlatformConditionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty platforms = property.FindPropertyRelative(nameof(PlatformCondition.Platforms));
            if (platforms.arraySize > 0)
            {
                SerializedProperty filter = property.FindPropertyRelative(nameof(PlatformCondition.Filter));
                PlatformCondition.FilterType  filterType = (PlatformCondition.FilterType) filter.enumValueIndex;
                label.text = $"{(filterType == PlatformCondition.FilterType.Include ? "Only on" : "Unless on")}: {string.Join(", ", platforms.IterateEnumArrayStrings())}";
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
