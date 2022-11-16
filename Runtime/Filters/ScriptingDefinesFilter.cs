#if UNITY_EDITOR
using System;
#if UNITY_2019_3_OR_NEWER
using UnityEditor.Compilation;
#endif

namespace Gilzoide.ConditionalObjects.Filters
{
    [Serializable]
    public class ScriptingDefineSymbolsFilter
    {
        public string[] DefineConstraints;

        public bool IsEmpty => DefineConstraints.Length <= 0;

        public bool Match(string[] defines)
        {
            return IsEmpty
                || AreConstraintsCompatible(defines, DefineConstraints);
        }

        public static bool AreConstraintsCompatible(string[] defines, string[] constraints)
        {
#if UNITY_2019_3_OR_NEWER
            return CompilationPipeline.IsDefineConstraintsCompatible(defines, constraints);
#else
            foreach (string constraint in constraints)
            {
                string trimmedConstraint = constraint.Trim();
                if (trimmedConstraint.StartsWith("!"))
                {
                    if (Array.IndexOf(defines, trimmedConstraint.Substring(1).TrimStart()) >= 0)
                    {
                        return false;
                    }
                }
                else if (Array.IndexOf(defines, trimmedConstraint) < 0)
                {
                    return false;
                }
            }
            return true;
#endif
        }
    }
}
#endif