using Gilzoide.ConditionalObjects.Filters;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public abstract class FilteredObjectModifier : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Conditions")]
        public EditorFilter EditorFilter;
        public DevelopmentFilter DevelopmentFilter;
        public PlatformFilter PlatformFilter;

        private void Start()
        {
            // no-op, but needed to be able to disable the component
        }

        public bool ShouldApply(bool isEditor, bool isDevelopment, BuildTarget buildTarget)
        {
            if (!enabled)
            {
                return false;
            }

            switch (GetApplyIncludeMode())
            {
                case IncludeMode.Include:
                    return IsIncluded(isEditor, isDevelopment, buildTarget);
                
                case IncludeMode.Exclude:
                    return IsExcluded(isEditor, isDevelopment, buildTarget);
                
                default:
                    return false;
            }
        }

        public abstract void Apply();

        protected bool IsIncluded(bool isEditor, bool isDevelopment, BuildTarget buildTarget)
        {
            return EditorFilter.IsIncluded(isEditor)
                && DevelopmentFilter.IsIncluded(isDevelopment)
                && PlatformFilter.IsPlatformIncluded(buildTarget);
        }

        protected bool IsExcluded(bool isEditor, bool isDevelopment, BuildTarget buildTarget)
        {
            return EditorFilter.IsExcluded(isEditor)
                && DevelopmentFilter.IsExcluded(isDevelopment)
                && PlatformFilter.IsPlatformExcluded(buildTarget);
        }
        
        protected abstract IncludeMode GetApplyIncludeMode();
#endif
    }
}
