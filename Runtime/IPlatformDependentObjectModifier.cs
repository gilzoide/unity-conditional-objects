using UnityEditor;

namespace Gilzoide.ConditionalObjects
{
    public interface IPlatformDependentObjectModifier
    {
#if UNITY_EDITOR
        void ApplyForTarget(BuildTarget buildTarget);
#endif
    }
}
