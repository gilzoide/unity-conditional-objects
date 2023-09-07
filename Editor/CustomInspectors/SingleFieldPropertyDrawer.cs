using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(ISingleFieldProperty), true)]
    public class SingleFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty field = GetFieldProperty(property);
            EditorGUI.PropertyField(position, field, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty field = GetFieldProperty(property);
            return EditorGUI.GetPropertyHeight(field, label, true);
        }

        private static SerializedProperty GetFieldProperty(SerializedProperty property)
        {
            SerializedProperty field = property.Copy();
            field.NextVisible(true);
            return field;
        }
    }
}
