using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Project.Utility
{
    public static class MethodExtensions
    {
        public static string RemoveQuotes(this string value)
        {
            return value.Replace("\"", "");
        }
 
        public static float TwoDecimals(this float value)
        {
            return Mathf.Round(value * 1000.0f) / 1000.0f;
        }
    }
}

