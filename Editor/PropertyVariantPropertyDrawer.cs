using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects.Editor
{
    [CustomPropertyDrawer(typeof(PropertyVariant))]
    public class PropertyVariantPropertyDrawer : PropertyDrawer
    {
        private static readonly GUIContent _valueTitle = new GUIContent("Value");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty target = property.FindPropertyRelative(nameof(PropertyVariant.Target));
            SerializedProperty propertyPath = property.FindPropertyRelative(nameof(PropertyVariant.PropertyPath));
            
            position.height = EditorGUI.GetPropertyHeight(target);
            EditorGUI.PropertyField(position, target);
            position.y += EditorGUIUtility.standardVerticalSpacing + position.height;

            using (new EditorGUI.DisabledScope(target.objectReferenceValue == null))
            {
                (string[] objectPropertyNames, int index) = GetProperties(target.objectReferenceValue, propertyPath.stringValue);
                int newIndex = EditorGUI.Popup(position, propertyPath.name, index, objectPropertyNames);
                if (newIndex >= 0 && newIndex < objectPropertyNames.Length)
                {
                    string selectedProperty = objectPropertyNames[newIndex].Replace("/", ".");
                    propertyPath.stringValue = selectedProperty;
                    position.y += EditorGUIUtility.standardVerticalSpacing + EditorStyles.popup.CalcHeight(GUIContent.none, position.width);
                    ShowInput(position, property, GetReferencedProperty(target, propertyPath));
                }
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty target = property.FindPropertyRelative(nameof(PropertyVariant.Target));
            SerializedProperty propertyPath = property.FindPropertyRelative(nameof(PropertyVariant.PropertyPath));
            float height = EditorGUI.GetPropertyHeight(target)
                + EditorGUIUtility.standardVerticalSpacing
                + EditorGUI.GetPropertyHeight(propertyPath);
            
            SerializedProperty referencedProperty = GetReferencedProperty(target, propertyPath);
            if (referencedProperty != null)
            {
                height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(GetVariantProperty(property, referencedProperty), true);
            }
            return height;
        }

        private void ShowInput(Rect position, SerializedProperty baseProperty, SerializedProperty referenceProperty)
        {
            switch (referenceProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Boolean:
                case SerializedPropertyType.Float:
                case SerializedPropertyType.String:
                case SerializedPropertyType.Color:
                case SerializedPropertyType.AnimationCurve:
                case SerializedPropertyType.Rect:
                case SerializedPropertyType.RectInt:
                case SerializedPropertyType.Bounds:
                case SerializedPropertyType.BoundsInt:
                case SerializedPropertyType.Vector2:
                case SerializedPropertyType.Vector3:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Vector2Int:
                case SerializedPropertyType.Vector3Int:
                case SerializedPropertyType.Quaternion:
#if UNITY_2021_1_OR_NEWER
                case SerializedPropertyType.Hash128:
#endif
                    EditorGUI.PropertyField(position, GetVariantProperty(baseProperty, referenceProperty), _valueTitle, true);
                    break;
                
                case SerializedPropertyType.Enum:
                {
                    SerializedProperty variantProperty = GetVariantProperty(baseProperty, referenceProperty);
                    if (referenceProperty.IsEnumFlags())
                    {
                        variantProperty.intValue = EditorGUI.MaskField(position, _valueTitle, variantProperty.intValue, referenceProperty.enumDisplayNames);
                    }
                    else
                    {
                        variantProperty.intValue = EditorGUI.Popup(position, _valueTitle.text, variantProperty.intValue, referenceProperty.enumDisplayNames);
                    }
                    break;
                }

                case SerializedPropertyType.ObjectReference:
                {
                    SerializedProperty variantProperty = GetVariantProperty(baseProperty, referenceProperty);
                    Type objectType = referenceProperty.FindObjectType();
                    variantProperty.ResetObjectIfTypeMismatches(objectType);
                    variantProperty.objectReferenceValue = EditorGUI.ObjectField(position, _valueTitle, variantProperty.objectReferenceValue, objectType, true);
                    break;
                }
            }
        }

        private SerializedProperty GetVariantProperty(SerializedProperty variantProperty, SerializedProperty referenceProperty)
        {
            switch (referenceProperty.propertyType)
            {
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Integer:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Integer));
                case SerializedPropertyType.Boolean:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Boolean));
                case SerializedPropertyType.Float:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Float));
                case SerializedPropertyType.String:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.String));
                case SerializedPropertyType.Color:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Color));
                case SerializedPropertyType.ObjectReference:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Object));
                case SerializedPropertyType.Vector2:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Vector2));
                case SerializedPropertyType.Vector3:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Vector3));
                case SerializedPropertyType.Vector4:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Vector4));
                case SerializedPropertyType.Vector2Int:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Vector2Int));
                case SerializedPropertyType.Vector3Int:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Vector3Int));
                case SerializedPropertyType.Rect:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Rect));
                case SerializedPropertyType.RectInt:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.RectInt));
                case SerializedPropertyType.AnimationCurve:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.AnimationCurve));
                case SerializedPropertyType.Bounds:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Bounds));
                case SerializedPropertyType.BoundsInt:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.BoundsInt));
                case SerializedPropertyType.Quaternion:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Quaternion));
#if UNITY_2021_1_OR_NEWER
                case SerializedPropertyType.Hash128:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Hash128));
#endif
                default:
                    throw new ArgumentOutOfRangeException(nameof(SerializedProperty.propertyType), $"Property of type {referenceProperty.propertyType} is not supported");
            }
        }

        private (string[] names, int currentIndex) GetProperties(Object obj, string currentProperty)
        {
            int index = 0;
            var subproperties = new List<string>();
            if (obj != null)
            {
                SerializedProperty childProperty = new SerializedObject(obj).GetIterator();
                if (childProperty.NextVisible(true))
                do
                {
                    if (currentProperty == childProperty.propertyPath)
                    {
                        index = subproperties.Count;
                    }
                    switch (childProperty.propertyType)
                    {
                        case SerializedPropertyType.Generic:
                        case SerializedPropertyType.ArraySize:
                            break;
                        
                        default:
                            subproperties.Add(childProperty.propertyPath.Replace(".", "/"));
                            break;
                    }
                } while (childProperty.NextVisible(true));
            }

            return (subproperties.ToArray(), index);
        }

        private SerializedProperty GetReferencedProperty(SerializedProperty target, SerializedProperty propertyPath)
        {
            return target.objectReferenceValue != null
                ? new SerializedObject(target.objectReferenceValue).FindProperty(propertyPath.stringValue)
                : null;
        }
    }
}
