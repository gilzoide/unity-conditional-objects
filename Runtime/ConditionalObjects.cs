using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public class ConditionalObjects : MonoBehaviour
    {
#if UNITY_EDITOR
        public Object[] TargetObjects;
        public List<BuildTarget> IncludeInPlatforms;
        public List<BuildTarget> ExcludeInPlatforms;

        public void ApplyForTarget(BuildTarget buildTarget)
        {
            if (ExcludeInPlatforms.Contains(buildTarget) || (IncludeInPlatforms.Count > 0 && !IncludeInPlatforms.Contains(buildTarget)))
            {
                foreach (Object obj in TargetObjects)
                {
                    DestroyImmediate(obj, true);
                }
            }
        }
#endif
    }
}
