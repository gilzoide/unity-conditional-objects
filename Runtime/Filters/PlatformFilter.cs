#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Gilzoide.ConditionalObjects.Filters
{
    [Serializable]
    public class PlatformFilter
    {
        public IncludeMode Filter;
        public List<BuildTarget> Platforms;

        public bool IsEmpty => Platforms.Count <= 0;

        public bool Match(BuildTarget buildTarget)
        {
            return IsEmpty
                || Filter == IncludeMode.Include && Platforms.Contains(buildTarget)
                || Filter == IncludeMode.Exclude && !Platforms.Contains(buildTarget);
        }
    }
}
#endif
