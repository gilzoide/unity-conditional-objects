using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class DevelopmentDependency
    {
#if UNITY_2020_2_OR_NEWER
        public static bool IsDevelopment { get; private set; }
        public static readonly string DependencyName = $"{typeof(DevelopmentDependency).FullName}.{nameof(IsDevelopment)}";

        [InitializeOnLoadMethod]
        private static void TrackDevelopmentBuild()
        {
            MarkIsDevelopment(EditorUserBuildSettings.development);
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (IsDevelopment != EditorUserBuildSettings.development)
            {
                MarkIsDevelopment(EditorUserBuildSettings.development);
            }
        }

        private static void MarkIsDevelopment(bool isDevelopment)
        {
            IsDevelopment = isDevelopment;
            AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(IsDevelopment.ToString()));
        }
#else
        public static bool IsDevelopment => EditorUserBuildSettings.development;
#endif
    }
}
