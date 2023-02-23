using Gilzoide.ConditionalObjects.Filters;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    public abstract class AImportTimeObjectModifier : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Conditions")]
        
        [Tooltip("Filter between Editor vs Build Players. Use None to bypass this filter")]
        public EditorFilter EditorFilter;
        
        [Tooltip("Filter between Development vs Release build configuration. Use None to bypass this filter")]
        public DevelopmentFilter DevelopmentFilter;

#if UNITY_2019_3_OR_NEWER
        [Tooltip("Filter based on scripting symbols defined when compiling the project. Negating symbols with `!` and the logical operator `||` (OR) are supported.")]
#else
        [Tooltip("Filter based on scripting symbols defined when compiling the project. Negating symbols with `!` is supported.")]
#endif
        public ScriptingDefineSymbolsFilter ScriptingDefineConstraints;
        
        [Tooltip("Filter based on the active build platform")]
        public PlatformFilter PlatformFilter;

        private void Start()
        {
            // no-op, but defined to be able to disable the component
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
