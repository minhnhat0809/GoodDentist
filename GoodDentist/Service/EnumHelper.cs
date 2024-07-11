using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));
                if (attribute != null && attribute.Description == description)
                {
                    return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException($"Not found: {description}", nameof(description));
        }

        public static string GetEnumDescriptionFromInt<T>(int value) where T : Enum
        {
            T enumValue = (T)Enum.ToObject(typeof(T), value);
            return GetDescription(enumValue);
        }

        public static int GetIntFromEnumDescription<T>(string description) where T : Enum
        {
            T enumValue = GetValueFromDescription<T>(description);
            return Convert.ToInt32(enumValue);
        }
    }
}
