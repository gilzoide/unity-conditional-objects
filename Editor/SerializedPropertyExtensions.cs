using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects.Editor
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

        private static IList<Type> ObjectSubclasses => _objectSubclasses != null ? _objectSubclasses : (_objectSubclasses = FindObjectSubclasses());
        private static IList<Type> _objectSubclasses;
        private static Regex _propertyTypeRegex = new Regex(@"\s*PPtr\W*(\w+)");

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
    }
}
