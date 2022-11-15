using UnityEditor;
#if UNITY_2017_3_OR_NEWER
using UnityEditor.Compilation;
#endif
using UnityEditor.Callbacks;
using UnityEngine;

namespace Gilzoide.ConditionalObjects.Editor
{
    public class ScriptingDefineSymbolsDependency
    {
#if UNITY_2020_2_OR_NEWER
        public static readonly string DependencyName = $"{typeof(ScriptingDefineSymbolsDependency).FullName}.{nameof(ScriptingDefineSymbols)}";

        [DidReloadScripts]
        static void OnDidReloadScripts()
        {
            AssetDatabase.RegisterCustomDependency(DependencyName, Hash128.Compute(string.Join(";", ScriptingDefineSymbols)));
        }
#endif

#if UNITY_2017_3_OR_NEWER
        public static string[] ScriptingDefineSymbols => CompilationPipeline.GetAssemblies()[0].defines;
#else
        public static string[] ScriptingDefineSymbols => PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(";");
#endif
    }
}
