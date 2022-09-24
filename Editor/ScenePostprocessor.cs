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
            IEnumerable<IPlatformDependentObjectModifier> foundComponents = Object.FindObjectsOfType<MonoBehaviour>()
                .OfType<IPlatformDependentObjectModifier>();
            
            BuildTarget selectedBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            foreach (IPlatformDependentObjectModifier conditionalObjects in foundComponents)
            {
                conditionalObjects.ApplyForTarget(selectedBuildTarget);
                Object.DestroyImmediate((Object) conditionalObjects, true);
            }
        }
    }
}
