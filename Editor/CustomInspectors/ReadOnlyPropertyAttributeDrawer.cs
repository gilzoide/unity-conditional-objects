using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyPropertyAttribute))]
    public class ReadOnlyPropertyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
