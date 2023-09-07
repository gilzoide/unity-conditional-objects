using System;
using System.Diagnostics;
using UnityEngine;

namespace Gilzoide.ConditionalObjects
{
    [Conditional("UNITY_EDITOR"), AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyPropertyAttribute : PropertyAttribute
    {
    }
}
