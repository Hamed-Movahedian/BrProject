    using UnityEngine;

    public static class MathfExtensions
    {
        public static int Between(this int value, int min, int max)
        {
            return Mathf.Max(Mathf.Min(value, max), min);
        }
        
        public static float Between(this float value, float min, float max)
        {
            return Mathf.Max(Mathf.Min(value, max), min);
        }
    }
