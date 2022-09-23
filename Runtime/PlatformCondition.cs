using System;
using System.Collections.Generic;
using UnityEditor;

namespace Gilzoide.ConditionalObjects
{
    [Serializable]
    public class PlatformCondition
    {
#if UNITY_EDITOR
        public enum FilterType
        {
            Include,
            Exclude,
        }
        public FilterType Filter;
        public List<BuildTarget> Platforms;

        public bool IsPlatformExcluded(BuildTarget buildTarget)
        {
            return Filter == FilterType.Exclude && Platforms.Contains(buildTarget)
                || Filter == FilterType.Include && !Platforms.Contains(buildTarget);
        }

        public bool IsPlatformIncluded(BuildTarget buildTarget)
        {
            return Filter == FilterType.Include && Platforms.Contains(buildTarget)
                || Filter == FilterType.Exclude && !Platforms.Contains(buildTarget);
        }
#endif
    }
}
