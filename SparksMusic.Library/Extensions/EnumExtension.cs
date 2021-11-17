using System.ComponentModel;

namespace SparksMusic.Library
{
    /// <summary>
    /// Enum extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieve description field from specific enum.
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns>The enum description.</returns>
        public static string GetDescription(this System.Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute),
                                                       false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
