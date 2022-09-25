using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
#if UNITY_2020_2_OR_NEWER
    public class EditorDependency : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public static bool IsEditor { get; private set; }

        public int callbackOrder => 0;

        public static readonly string DependencyName = $"{typeof(EditorDependency).FullName}.{nameof(IsEditor)}";

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            MarkIsEditor(!BuildPipeline.isBuildingPlayer);
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            MarkIsEditor(false);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            MarkIsEditor(true);
        }

        private static void MarkIsEditor(bool isEditor)
        {
            IsEditor = isEditor;
            AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(IsEditor.ToString()));
        }
    }
#else
    public class EditorDependency
    {
        public static bool IsEditor => BuildPipeline.isBuildingPlayer;
    }
#endif
}
