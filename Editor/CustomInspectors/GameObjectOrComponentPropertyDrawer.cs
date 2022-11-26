using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(GameObjectOrComponent))]
    public class GameObjectOrComponentPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty gameObject = property.FindPropertyRelative(nameof(GameObjectOrComponent.GameObject));
            SerializedProperty componentIndex = property.FindPropertyRelative(nameof(GameObjectOrComponent.ComponentIndex));

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, gameObject);
            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(gameObject);

            ShowComponentInput(position, gameObject.objectReferenceValue as GameObject, componentIndex);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty gameObject = property.FindPropertyRelative(nameof(GameObjectOrComponent.GameObject));
            SerializedProperty componentIndex = property.FindPropertyRelative(nameof(GameObjectOrComponent.ComponentIndex));

            return EditorGUI.GetPropertyHeight(gameObject, true)
                + EditorGUIUtility.standardVerticalSpacing
                + EditorGUI.GetPropertyHeight(componentIndex, true);
        }

        private void ShowComponentInput(Rect position, GameObject gameObject, SerializedProperty componentIndexProperty)
        {
            var componentNames = new List<string>();
            int selectedIndex;
            if (gameObject != null)
            {
                componentNames.Add("GameObject");
                foreach (Component component in gameObject.GetAllComponents())
                {
                    string typeName = component.GetType().Name + " ";
                    int nameCount = componentNames.Count(name => name.StartsWith(typeName));
                    if (nameCount > 0)
                    {
                        typeName += $"({nameCount})";
                    }
                    componentNames.Add(typeName);
                }
                selectedIndex = componentIndexProperty.intValue;
            }
            else
            {
                selectedIndex = -1;
            }

            using (new EditorGUI.DisabledScope(gameObject == null))
            {
                componentIndexProperty.intValue = EditorGUI.Popup(position, "Target", selectedIndex + 1, componentNames.ToArray()) - 1;
            }
        }
    }
}
