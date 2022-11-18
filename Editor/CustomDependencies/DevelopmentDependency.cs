using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class DevelopmentDependency
    {
#if UNITY_2020_2_OR_NEWER
        public static bool IsDevelopment
        {
            get => _isDevelopment;
            internal set => MarkIsDevelopment(value);
        }
        public static readonly string DependencyName = $"{typeof(DevelopmentDependency).FullName}.{nameof(IsDevelopment)}";
        
        private static bool _isDevelopment;

        [InitializeOnLoadMethod]
        private static void TrackDevelopmentBuild()
        {
            MarkIsDevelopment(EditorUserBuildSettings.development);
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (!BuildPipeline.isBuildingPlayer && IsDevelopment != EditorUserBuildSettings.development)
            {
                MarkIsDevelopment(EditorUserBuildSettings.development);
            }
        }

        private static void MarkIsDevelopment(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
            AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(isDevelopment.ToString()));
        }
#else
        public static bool IsDevelopment => EditorUserBuildSettings.development;
#endif
    }
}
