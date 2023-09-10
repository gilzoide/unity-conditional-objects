using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class ObjectExtensions
    {
        public static bool IsPartOfPrefab(this Object obj)
        {
            if (obj is GameObject go)
            {
                return string.IsNullOrWhiteSpace(go.scene.path);
            }
            if (obj is Component component)
            {
                return string.IsNullOrWhiteSpace(component.gameObject.scene.path);
            }
            return false;
        }

        public static Component[] GetAllComponents(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.GetComponents<Component>() : new Component[0];
        }
    }
}
