using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class CustomPropertyDrawerFinder
    {
        public static PropertyDrawer FindPropertyDrawer(FieldInfo field)
        {
            if (field == null || PropertyDrawer_Attribute == null || PropertyDrawer_FieldInfo == null)
            {
                return null;
            }

            if (_propertyDrawerForField.TryGetValue(field, out PropertyDrawer propertyDrawer))
            {
                return propertyDrawer;
            }

            PropertyAttribute attribute = field.GetCustomAttribute<PropertyAttribute>();
            Type propertyDrawerType = FindCustomPropertyDrawer(attribute);
            if (propertyDrawerType == null)
            {
                _propertyDrawerForField[field] = null;
                return null;
            }

            propertyDrawer = (PropertyDrawer) Activator.CreateInstance(propertyDrawerType);
            PropertyDrawer_Attribute.SetValue(propertyDrawer, attribute);
            PropertyDrawer_FieldInfo.SetValue(propertyDrawer, field);
            _propertyDrawerForField[field] = propertyDrawer;
            return propertyDrawer;
        }

        public static Type FindCustomPropertyDrawer(PropertyAttribute attribute)
        {
            if (attribute == null || CustomPropertyDrawer_Type == null || CustomPropertyDrawer_UseForChildren == null)
            {
                return null;
            }

            foreach (Type propertyDrawerType in CustomPropertyDrawerTypes)
            {
                if (!propertyDrawerType.IsSubclassOf(typeof(PropertyDrawer)))
                {
                    continue;
                }
                foreach (CustomPropertyDrawer customPropertyDrawer in propertyDrawerType.GetCustomAttributes<CustomPropertyDrawer>())
                {
                    Type drawnType = CustomPropertyDrawer_Type.GetValue(customPropertyDrawer) as Type;
                    Type attributeType = attribute.GetType();
                    if (drawnType == attributeType || ((bool) CustomPropertyDrawer_UseForChildren.GetValue(customPropertyDrawer)) && drawnType.IsAssignableFrom(attributeType))
                    {
                        return propertyDrawerType;
                    }
                }
            }
            return null;
        }

        private static readonly Dictionary<FieldInfo, PropertyDrawer> _propertyDrawerForField = new Dictionary<FieldInfo, PropertyDrawer>();

        private static IList<Type> CustomPropertyDrawerTypes => _customPropertyDrawerTypes != null ? _customPropertyDrawerTypes : (_customPropertyDrawerTypes = FindCustomPropertyDrawerTypes());
        private static IList<Type> _customPropertyDrawerTypes;

        private static IList<Type> FindCustomPropertyDrawerTypes()
        {
#if UNITY_2019_2_OR_NEWER
            return TypeCache.GetTypesWithAttribute<CustomPropertyDrawer>();
#else
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => type.GetCustomAttributes<CustomPropertyDrawer>().Any())
                .ToList();
#endif
        }

        private static readonly FieldInfo CustomPropertyDrawer_Type = typeof(CustomPropertyDrawer).GetField("m_Type", _bindingFlags);
        private static readonly FieldInfo CustomPropertyDrawer_UseForChildren = typeof(CustomPropertyDrawer).GetField("m_UseForChildren", _bindingFlags);
        private static readonly FieldInfo PropertyDrawer_Attribute = typeof(PropertyDrawer).GetField("m_Attribute", _bindingFlags);
        private static readonly FieldInfo PropertyDrawer_FieldInfo = typeof(PropertyDrawer).GetField("m_FieldInfo", _bindingFlags);

        private const BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    }
}
