using System;

namespace Gilzoide.ConditionalObjects.Filters
{
    public enum DevelopmentFilter
    {
        None,
        Development,
        Release,
    }

    public static class DevelopmentFilterExtensions
    {
        public static bool Match(this DevelopmentFilter filter, bool isDevelopment)
        {
            switch (filter)
            {
                case DevelopmentFilter.None:
                    return true;

                case DevelopmentFilter.Development:
                    return isDevelopment;
                
                case DevelopmentFilter.Release:
                    return !isDevelopment;

                default:
                    throw new ArgumentOutOfRangeException(nameof(filter));
            }
        }
    }
}
