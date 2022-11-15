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
        public static bool Match(this EditorFilter filter, bool isEditor)
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
    }
}
