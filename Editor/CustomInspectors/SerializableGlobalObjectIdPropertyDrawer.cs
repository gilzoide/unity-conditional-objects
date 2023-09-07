using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(SerializableGlobalObjectId))]
    public class SerializableGlobalObjectIdPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, GetTextProperty(property), label);
                if (TryGetGlobalObjectId(property, out GlobalObjectId globalObjectId))
                {
                    EditorGUI.indentLevel++;
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.ObjectField(position, "Asset", AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(globalObjectId.assetGUID)), typeof(Object), false);
                    if (GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId) is Object obj)
                    {
                        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.ObjectField(position, "Object", obj, typeof(Object), true);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label);
            if (TryGetGlobalObjectId(property, out GlobalObjectId globalObjectId))
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalObjectId) is Object obj)
                {
                    height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }

        private static SerializedProperty GetTextProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("_globalObjectIdText");
        }

        private static bool TryGetGlobalObjectId(SerializedProperty property, out GlobalObjectId globalObjectId)
        {
            string globalObjectIdText = GetTextProperty(property).stringValue;
            return GlobalObjectId.TryParse(globalObjectIdText, out globalObjectId);
        }
    }
}
