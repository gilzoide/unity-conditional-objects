using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class ScenePostprocessor
    {
        [PostProcessScene]
        static void OnPostprocessScene()
        {
            MonoBehaviour[] scripts = Object.FindObjectsOfType<MonoBehaviour>();
            
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            foreach (IPlatformDependentObjectModifier conditionalObjects in scripts.OfType<IPlatformDependentObjectModifier>())
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }

            bool isDevelopment = DevelopmentDependency.IsDevelopment;
            foreach (IDevelopmentDependentObjectModifier conditionalObjects in scripts.OfType<IDevelopmentDependentObjectModifier>())
            {
                conditionalObjects.Apply(isDevelopment);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }
        }
    }
}
