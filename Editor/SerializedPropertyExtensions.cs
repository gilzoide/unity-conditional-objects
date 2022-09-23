using System.Collections.Generic;
using UnityEditor;

namespace Gilzoide.ConditionalObjects.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static IEnumerable<string> IterateEnumArrayStrings(this SerializedProperty arrayProperty)
        {
            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                yield return element.enumValueIndex >= 0 && element.enumValueIndex < element.enumDisplayNames.Length
                    ? element.enumDisplayNames[element.enumValueIndex]
                    : "";
            }
        }
    }
}
