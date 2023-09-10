using System;
using System.Collections.Generic;
using Gilzoide.ConditionalObjects.Internal;
using UnityEditor;
using UnityEditorInternal;
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
            
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, target);
            position.y += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(target);

            Object targetObject = target.objectReferenceValue;
            using (new EditorGUI.DisabledScope(targetObject == null))
            {
                (string[] objectPropertyNames, int index) = GetProperties(targetObject, propertyPath.stringValue);
                position.height = EditorGUI.GetPropertyHeight(propertyPath);
                int newIndex = EditorGUI.Popup(position, "Property Path", index, objectPropertyNames);
                if (newIndex >= 0 && newIndex < objectPropertyNames.Length)
                {
                    string selectedProperty = objectPropertyNames[newIndex].Replace("/", ".");
                    propertyPath.stringValue = selectedProperty;
                    position.y += EditorGUIUtility.standardVerticalSpacing + EditorStyles.popup.CalcHeight(GUIContent.none, position.width);
                    
                    SerializedProperty referencedProperty = GetReferencedProperty(targetObject, propertyPath);
                    SerializedProperty variantProperty = GetVariantProperty(property, referencedProperty);
                    if (newIndex != index)
                    {
                        CopySerializedProperty(variantProperty, referencedProperty);
                    }
                    position.height = EditorGUI.GetPropertyHeight(referencedProperty);
                    DrawVariantProperty(position, variantProperty, referencedProperty, _valueTitle);
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
            
            SerializedProperty referencedProperty = GetReferencedProperty(target.objectReferenceValue, propertyPath);
            if (referencedProperty != null)
            {
                height += EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(referencedProperty, true);
            }
            return height;
        }

        private static void DrawVariantProperty(Rect position, SerializedProperty variantProperty, SerializedProperty referenceProperty, GUIContent label)
        {
            PropertyDrawer propertyDrawer = CustomPropertyDrawerFinder.FindPropertyDrawer(referenceProperty.FindFieldInfo());
            if (propertyDrawer != null)
            {
                propertyDrawer.OnGUI(position, variantProperty, label);
                return;
            }

            if (referenceProperty.serializedObject.targetObject is GameObject)
            {
                if (referenceProperty.propertyPath == "m_TagString")
                {
                    variantProperty.stringValue = EditorGUI.TagField(position, label, variantProperty.stringValue);
                    return;
                }
                if (referenceProperty.propertyPath == "m_Layer")
                {
                    variantProperty.intValue = EditorGUI.LayerField(position, label, variantProperty.intValue);
                    return;
                }
                if (referenceProperty.propertyPath == "m_StaticEditorFlags")
                {
                    variantProperty.intValue = EditorGUI.MaskField(position, label, variantProperty.intValue, typeof(StaticEditorFlags).GetEnumNames());
                    return;
                }
            }

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
                case SerializedPropertyType.Gradient:
#if UNITY_2021_1_OR_NEWER
                case SerializedPropertyType.Hash128:
#endif
                    EditorGUI.PropertyField(position, variantProperty, label, true);
                    break;
                
                case SerializedPropertyType.Enum:
                    if (referenceProperty.IsEnumFlags())
                    {
                        variantProperty.intValue = EditorGUI.MaskField(position, label, variantProperty.intValue, referenceProperty.enumDisplayNames);
                    }
                    else
                    {
                        variantProperty.intValue = EditorGUI.Popup(position, label.text, variantProperty.intValue, referenceProperty.enumDisplayNames);
                    }
                    break;
                
                case SerializedPropertyType.LayerMask:
                {
                    int layerMask = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(variantProperty.intValue);
                    layerMask = EditorGUI.MaskField(position, label, layerMask, InternalEditorUtility.layers);
                    variantProperty.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(layerMask);
                    break;
                }

                case SerializedPropertyType.ObjectReference:
                {
                    Type objectType = referenceProperty.FindObjectType();
                    variantProperty.ResetObjectIfTypeMismatches(objectType);
                    variantProperty.objectReferenceValue = EditorGUI.ObjectField(position, label, variantProperty.objectReferenceValue, objectType, true);
                    break;
                }

                case SerializedPropertyType.Character:
                {
                    string c = EditorGUI.TextField(position, label, variantProperty.stringValue);
                    if (c.Length > 1)
                    {
                        c = c.Substring(0, 1);
                    }
                    variantProperty.stringValue = c;
                    break;
                }
            }
        }

        private SerializedProperty GetVariantProperty(SerializedProperty variantProperty, SerializedProperty referenceProperty)
        {
            switch (referenceProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.LayerMask:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Integer));
                case SerializedPropertyType.Boolean:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Boolean));
                case SerializedPropertyType.Float:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Float));
                case SerializedPropertyType.String:
                case SerializedPropertyType.Character:
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
                case SerializedPropertyType.Gradient:
                    return variantProperty.FindPropertyRelative(nameof(PropertyVariant.Gradient));
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

        public static void CopySerializedProperty(SerializedProperty property, SerializedProperty source)
        {
            switch (source.propertyType)
            {
                case SerializedPropertyType.Enum:
#if UNITY_2021_1_OR_NEWER
                    property.intValue = source.IsEnumFlags()
                        ? source.enumValueFlag
                        : source.enumValueIndex;
#else
                    property.intValue = source.enumValueIndex;
#endif
                    break;
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                    property.intValue = source.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = source.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = source.floatValue;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = source.stringValue;
                    break;
                case SerializedPropertyType.Character:
                    property.stringValue = new string(new[] { (char) source.intValue });
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = source.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = source.objectReferenceValue;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = source.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = source.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = source.vector4Value;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = source.vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = source.vector3IntValue;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = source.rectValue;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = source.rectIntValue;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = source.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = source.boundsValue;
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = source.boundsIntValue;
                    break;
                case SerializedPropertyType.Gradient:
                    property.SetGradient(source.GetGradient());
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = source.quaternionValue;
                    break;
#if UNITY_2021_1_OR_NEWER
                case SerializedPropertyType.Hash128:
                    property.hash128Value = source.hash128Value;
                    break;
#endif
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

        private SerializedProperty GetReferencedProperty(Object target, SerializedProperty propertyPath)
        {
            return target != null
                ? new SerializedObject(target).FindProperty(propertyPath.stringValue)
                : null;
        }
    }
}
