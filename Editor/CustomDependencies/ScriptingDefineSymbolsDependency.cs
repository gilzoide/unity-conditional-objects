using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_2017_3_OR_NEWER
using UnityEditor.Compilation;
#endif
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class ScriptingDefineSymbolsDependency
    {
#if UNITY_2017_3_OR_NEWER
        public static string[] ScriptingDefineSymbols => CompilationPipeline.GetAssemblies()[0].defines;
#else
        public static string[] ScriptingDefineSymbols => PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(";");
#endif

#if UNITY_2020_2_OR_NEWER
        public static readonly string DependencyName = $"{typeof(ScriptingDefineSymbolsDependency).FullName}.{nameof(ScriptingDefineSymbols)}";

        [DidReloadScripts]
        public static void RefreshDependency()
        {
            Hash128 hash = Hash128.Compute(string.Join(";", ScriptingDefineSymbols));
            AssetDatabase.RegisterCustomDependency(DependencyName, hash);
        }
#endif
    }
}
