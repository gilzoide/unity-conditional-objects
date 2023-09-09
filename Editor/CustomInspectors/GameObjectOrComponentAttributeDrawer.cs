using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectOrComponentAttribute))]
    public class SelectComponentAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            position.height = EditorGUIUtility.singleLineHeight;

            GameObject gameObject = null;
            Object targetObject = property.objectReferenceValue;
            if (targetObject is Component component)
            {
                EditorGUI.BeginChangeCheck();
                component = (Component) EditorGUI.ObjectField(position, label, component, typeof(Component), true);
                if (EditorGUI.EndChangeCheck())
                {
                    targetObject = component;
                }
                if (component)
                {
                    gameObject = component.gameObject;
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                gameObject = (GameObject) EditorGUI.ObjectField(position, label, targetObject as GameObject, typeof(GameObject), true);
                if (EditorGUI.EndChangeCheck())
                {
                    targetObject = gameObject;
                }
            }

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.indentLevel++;
            property.objectReferenceValue = ShowComponentInput(position, gameObject, targetObject);
            EditorGUI.indentLevel--;    
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label, true);
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }

        private Object ShowComponentInput(Rect position, GameObject gameObject, Object currentSelected)
        {
            var componentNames = new List<string>();
            int selectedIndex = -1;
            Component[] allComponents = gameObject.GetAllComponents();
            if (gameObject != null)
            {
                componentNames.Add(nameof(GameObject));
                for (int i = 0; i < allComponents.Length; i++)
                {
                    Component component = allComponents[i];
                    string typeName = component.GetType().Name + " ";
                    int nameCount = componentNames.Count(name => name.StartsWith(typeName));
                    if (nameCount > 0)
                    {
                        typeName += $"({nameCount})";
                    }
                    componentNames.Add(typeName);
                    
                    if (component == currentSelected)
                    {
                        selectedIndex = i;
                    }
                }
            }

            using (new EditorGUI.DisabledScope(gameObject == null))
            {
                selectedIndex = EditorGUI.Popup(position, "Component", selectedIndex + 1, componentNames.ToArray()) - 1;
            }
            return selectedIndex >= 0 ? allComponents[selectedIndex] : gameObject;
        }
    }
}
