using System;
using UnityEngine;

namespace Domain.Common.System
{
    public class EnumUtility
    {
        public static T GetValue<T>(int value)
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values[value];
        }
    }
}
