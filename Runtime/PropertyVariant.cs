using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects
{
    [Serializable]
    public class PropertyVariant
    {
        public Object Target;
        public string PropertyPath;

        public bool Boolean;
        public long Integer;
        public double Float;
        public string String;
        public Color Color;
        public Object Object;
        public Vector2 Vector2;
        public Vector3 Vector3;
        public Vector4 Vector4;
        public Vector2Int Vector2Int;
        public Vector3Int Vector3Int;
        public Rect Rect;
        public RectInt RectInt;
        public AnimationCurve AnimationCurve;
        public Bounds Bounds;
        public BoundsInt BoundsInt;
        public Quaternion Quaternion;
#if UNITY_2021_1_OR_NEWER
        public Hash128 Hash128;
#endif

#if UNITY_EDITOR
        public void Apply()
        {
            SerializedObject obj = new SerializedObject(Target);
            SerializedProperty property = obj.FindProperty(PropertyPath);
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.longValue = Integer;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = Boolean;
                    break;
                case SerializedPropertyType.Float:
                    property.doubleValue = Float;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = String;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = Color;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = Object;
                    break;
                case SerializedPropertyType.LayerMask:
                    property.intValue = (int) Integer;
                    break;
                case SerializedPropertyType.Enum:
                    if (property.IsEnumFlags())
                    {
                        property.enumValueFlag = (int) Integer;
                    }
                    else
                    {
                        property.enumValueIndex = (int) Integer;
                    }
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = Vector2;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = Vector3;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = Vector4;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = Vector2Int;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = Vector3Int;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = Rect;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = RectInt;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = AnimationCurve;
                    break;
                case SerializedPropertyType.Character:
                    property.intValue = string.IsNullOrEmpty(String) ? 0 : String[0];
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = Bounds;
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = BoundsInt;
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = Quaternion;
                    break;
#if UNITY_2021_1_OR_NEWER
                case SerializedPropertyType.Hash128:
                    property.hash128Value = Hash128;
                    break;
#endif
                default:
                    throw new ArgumentOutOfRangeException(nameof(SerializedProperty.propertyType), $"Property of type {property.propertyType} is not supported");
            }
            obj.ApplyModifiedProperties();
        }
#endif
    }
}
