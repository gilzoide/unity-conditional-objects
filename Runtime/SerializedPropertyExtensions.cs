#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects
{
    public static class SerializedPropertyExtensions
    {
        public static IEnumerable<string> IterateEnumArrayStrings(this SerializedProperty arrayProperty)
        {
            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                yield return element.enumValueIndex >= 0 && element.enumValueIndex < element.enumDisplayNames.Length
                    ? element.enumDisplayNames[element.enumValueIndex]
                    : "";
            }
        }

        public static Type FindObjectType(this SerializedProperty property)
        {
            Match typeNameMatch = _propertyTypeRegex.Match(property.type);
            if (typeNameMatch.Success)
            {
                string typeName = typeNameMatch.Groups[1].Value;
                return typeName == nameof(Object)
                    ? typeof(Object)
                    : ObjectSubclasses.First(type => type.Name == typeName);
            }

            Debug.LogError($"Could not find Object type for property '{property.serializedObject.targetObject.GetType()}.{property.propertyPath}'");
            return typeof(Object);
        }

        public static Type FindType(this SerializedProperty property)
        {
            Type type = property.serializedObject.targetObject.GetType();
            string cacheKey = $"{type}.{property.propertyPath}";
            if (_propertyTypesCache.TryGetValue(cacheKey, out Type ret))
            {
                return ret;
            }

            foreach (string propertyName in property.propertyPath.Split('.'))
            {
                if (type.GetProperty(propertyName, _bindingFlags) is PropertyInfo prop)
                {
                    type = prop.PropertyType;
                }
                else if (type.GetField(propertyName, _bindingFlags) is FieldInfo field)
                {
                    type = field.FieldType;
                }
                else
                {
                    type = null;
                    break;
                }
            }

            _propertyTypesCache[cacheKey] = type;
            return type;
        }

        public static FieldInfo FindFieldInfo(this SerializedProperty property)
        {
            Type type = property.serializedObject.targetObject.GetType();
            string cacheKey = $"{type}.{property.propertyPath}";
            if (_propertyFieldInfoCache.TryGetValue(cacheKey, out FieldInfo ret))
            {
                return ret;
            }

            FieldInfo fieldInfo = null;
            foreach (string propertyName in property.propertyPath.Split('.'))
            {
                if (type.GetProperty(propertyName, _bindingFlags) is PropertyInfo prop)
                {
                    type = prop.PropertyType;
                }
                else if (type.GetField(propertyName, _bindingFlags) is FieldInfo field)
                {
                    fieldInfo = field;
                    type = field.FieldType;
                }
                else
                {
                    type = null;
                    break;
                }
            }

            _propertyFieldInfoCache[cacheKey] = fieldInfo;
            return fieldInfo;
        }

        public static void ResetObjectIfTypeMismatches(this SerializedProperty property, Type objectType)
        {
            if (property.objectReferenceValue == null)
            {
                return;
            }

            Type type = property.objectReferenceValue.GetType(); 
            if (type != objectType && !type.IsSubclassOf(objectType))
            {
                property.objectReferenceValue = null;
            }
        }

        public static bool IsEnumFlags(this SerializedProperty property)
        {
            return property.FindType() is Type enumType
                ? enumType.GetCustomAttribute<FlagsAttribute>() != null
                : false;
        }

        public static Gradient GetGradient(this SerializedProperty property)
        {
#if UNITY_2022_1_OR_NEWER
            return property.gradientValue;
#else
            return (Gradient) SerializedProperty_gradientValue.GetValue(property);
#endif
        }

        public static void SetGradient(this SerializedProperty property, Gradient gradient)
        {
#if UNITY_2022_1_OR_NEWER
            property.gradientValue = gradient;
#else
            SerializedProperty_gradientValue.SetValue(property, gradient);
#endif
        }

        private const BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private static IList<Type> ObjectSubclasses => _objectSubclasses != null ? _objectSubclasses : (_objectSubclasses = FindObjectSubclasses());
        private static IList<Type> _objectSubclasses;
        private static readonly Regex _propertyTypeRegex = new Regex(@"\s*PPtr\W*(\w+)");

        private static IList<Type> FindObjectSubclasses()
        {
#if UNITY_2019_2_OR_NEWER
            return TypeCache.GetTypesDerivedFrom<Object>();
#else
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Object)))
                .ToList();
#endif
        }

        private static readonly Dictionary<string, Type> _propertyTypesCache = new Dictionary<string, Type>();
        private static readonly Dictionary<string, FieldInfo> _propertyFieldInfoCache = new Dictionary<string, FieldInfo>();

#if !UNITY_2022_1_OR_NEWER
        private static readonly PropertyInfo SerializedProperty_gradientValue = typeof(SerializedProperty).GetProperty("gradientValue", _bindingFlags);
#endif
    }
}
#endif