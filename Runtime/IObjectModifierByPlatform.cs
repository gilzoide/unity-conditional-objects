using UnityEditor;

namespace Gilzoide.ConditionalObjects
{
    public interface IObjectModifierByPlatform
    {
#if UNITY_EDITOR
        void ApplyForTarget(BuildTarget buildTarget);
#endif
    }
}
