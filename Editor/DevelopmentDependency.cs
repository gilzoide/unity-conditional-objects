using UnityEditor;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class DevelopmentDependency
    {
        public static bool IsDevelopment { get; private set; }
        public static readonly string DependencyName = $"{typeof(DevelopmentDependency).FullName}.{nameof(IsDevelopment)}";

        [InitializeOnLoadMethod]
        private static void TrackDevelopmentBuild()
        {
            IsDevelopment = EditorUserBuildSettings.development;
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (IsDevelopment != EditorUserBuildSettings.development)
            {
                IsDevelopment = EditorUserBuildSettings.development;
                AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(IsDevelopment.ToString()));
                AssetDatabase.Refresh();
            }
        }
    }
}
