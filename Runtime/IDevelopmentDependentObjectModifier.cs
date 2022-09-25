namespace Gilzoide.ConditionalObjects
{
    public interface IDevelopmentDependentObjectModifier
    {
#if UNITY_EDITOR
        void Apply(bool isDevelopment);
#endif
    }
}
