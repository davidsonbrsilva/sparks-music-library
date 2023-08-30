using System;
using System.ComponentModel;

namespace SparksMusic.Library.Utils
{
    public static class EnumUtils
    {
        public static T Parse<T>(string input) where T : System.Enum
        {
            var fields = typeof(T).GetFields();

            foreach (var field in fields)
            {
                var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == input)
                    return (T)Enum.Parse(typeof(T), field.Name);
            }

            foreach (var field in fields)
            {
                if (field.Name == input)
                    return (T)Enum.Parse(typeof(T), field.Name);
            }

            throw new Exception("Not found");
        }
    }
}
