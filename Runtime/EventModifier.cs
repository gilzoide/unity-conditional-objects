using UnityEngine;
using UnityEngine.Events;

namespace Gilzoide.ConditionalObjects
{
    public class EventModifier : AImportTimeObjectModifier
    {
#if UNITY_EDITOR
        [Header("Events")]
        [Tooltip("Event called on import if filters match")]
        public UnityEvent OnFiltersMatch;
        
        [Tooltip("Event called on import if filters don't match")]
        public UnityEvent OnFiltersDontMatch;

        protected override void Apply(bool filtersMatch)
        {
            if (filtersMatch)
            {
                OnFiltersMatch.Invoke();
            }
            else
            {
                OnFiltersDontMatch.Invoke();
            }
        }

        protected void OnValidate()
        {
            for (int i = 0; i < OnFiltersMatch.GetPersistentEventCount(); i++)
            {
                if (OnFiltersMatch.GetPersistentListenerState(i) == UnityEventCallState.RuntimeOnly)
                {
                    OnFiltersMatch.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
                }
            }
            for (int i = 0; i < OnFiltersDontMatch.GetPersistentEventCount(); i++)
            {
                if (OnFiltersDontMatch.GetPersistentListenerState(i) == UnityEventCallState.RuntimeOnly)
                {
                    OnFiltersDontMatch.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
                }
            }
        }
#endif
    }
}
