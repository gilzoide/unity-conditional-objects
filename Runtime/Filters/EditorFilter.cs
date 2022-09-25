using System;

namespace Gilzoide.ConditionalObjects.Filters
{
    public enum EditorFilter
    {
        None,
        Editor,
        BuiltPlayer
    }

    public static class EditorFilterExtensions
    {
        public static bool IsIncluded(this EditorFilter filter, bool isEditor)
        {
            switch (filter)
            {
                case EditorFilter.None:
                    return true;

                case EditorFilter.Editor:
                    return isEditor;
                
                case EditorFilter.BuiltPlayer:
                    return !isEditor;

                default:
                    throw new ArgumentOutOfRangeException(nameof(filter));
            }
        }

        public static bool IsExcluded(this EditorFilter filter, bool isEditor)
        {
            switch (filter)
            {
                case EditorFilter.None:
                    return true;

                case EditorFilter.Editor:
                    return !isEditor;
                
                case EditorFilter.BuiltPlayer:
                    return isEditor;

                default:
                    throw new ArgumentOutOfRangeException(nameof(filter));
            }
        }
    }
}
