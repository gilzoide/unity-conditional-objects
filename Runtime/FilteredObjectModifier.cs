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
        public ScriptingDefineSymbolsFilter ScriptingDefineConstraints;
        public PlatformFilter PlatformFilter;

        private void Start()
        {
            // no-op, but needed to be able to disable the component
        }

        public void Apply(bool isEditor, bool isDevelopment, BuildTarget buildTarget, string[] scriptingDefineSymbols)
        {
            if (!enabled)
            {
                return;
            }

            bool filtersMatch = EditorFilter.Match(isEditor)
                && DevelopmentFilter.Match(isDevelopment)
                && PlatformFilter.Match(buildTarget)
                && ScriptingDefineConstraints.Match(scriptingDefineSymbols);

            Apply(filtersMatch);
        }

        protected abstract void Apply(bool filtersMatch);
#endif
    }
}
