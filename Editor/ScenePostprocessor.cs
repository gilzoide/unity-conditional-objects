using System.Collections.Generic;
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
            IEnumerable<IObjectModifierByPlatform> foundComponents = Object.FindObjectsOfType<MonoBehaviour>()
                .OfType<IObjectModifierByPlatform>();
            
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            foreach (IObjectModifierByPlatform conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }
        }
    }
}
