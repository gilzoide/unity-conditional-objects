using System;
using UnityEditor;
#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif
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

#if UNITY_EDITOR
        public static string GetAssetPath(this GameObject gameObject)
        {
            if (!string.IsNullOrEmpty(gameObject.scene.path))
            {
                return gameObject.scene.path;
            }
            else if (PrefabStageUtility.GetCurrentPrefabStage() is PrefabStage prefabStage && prefabStage.IsPartOfPrefabContents(gameObject))
            {
#if UNITY_2020_1_OR_NEWER
                return prefabStage.assetPath;
#else
                return prefabStage.prefabAssetPath;
#endif
            }
            else
            {
                return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            }
        }
#endif
    }
}
