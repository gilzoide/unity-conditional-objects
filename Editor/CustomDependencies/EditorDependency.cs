using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class EditorDependency
    {
#if UNITY_2020_2_OR_NEWER
        public static bool IsEditor
        {
            get => _isEditor;
            internal set => MarkIsEditor(value);
        }
        public static readonly string DependencyName = $"{typeof(EditorDependency).FullName}.{nameof(IsEditor)}";

        private static bool _isEditor;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            MarkIsEditor(!BuildPipeline.isBuildingPlayer);
        }

        private static void MarkIsEditor(bool isEditor)
        {
            _isEditor = isEditor;
            AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(isEditor.ToString()));
        }
#else
        public static bool IsEditor => !BuildPipeline.isBuildingPlayer;
#endif
    }
}
