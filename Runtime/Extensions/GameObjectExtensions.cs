using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gilzoide.ConditionalObjects
{
    public static class GameObjectExtensions
    {
        public static Component[] GetAllComponents(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.GetComponents<Component>() : Array.Empty<Component>();
        }
        
        public static Object ComponentAtIndexOrSelf(this GameObject gameObject, int index)
        {
            Component[] components = gameObject.GetAllComponents();
            if (index >= 0 && index < components.Length)
            {
                return components[index];
            }
            else
            {
                return gameObject;
            }
        }
    }
}
