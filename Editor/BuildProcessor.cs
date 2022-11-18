#if UNITY_2020_2_OR_NEWER
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            EditorDependency.IsEditor = false;
            DevelopmentDependency.IsDevelopment = (report.summary.options & BuildOptions.Development) != 0;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            EditorDependency.IsEditor = true;
        }
    }
}
#endif