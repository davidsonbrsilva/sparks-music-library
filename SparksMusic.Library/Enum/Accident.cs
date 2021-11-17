using System.ComponentModel;

namespace SparksMusic.Library
{
    /// <summary>
    /// Accident enum
    /// </summary>
    public enum Accident
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// Double Flat
        /// </summary>
        [Description("bb")]
        DoubleFlat,

        /// <summary>
        /// Flat
        /// </summary>
        [Description("b")]
        Flat,

        /// <summary>
        /// Sharp
        /// </summary>
        [Description("#")]
        Sharp,

        /// <summary>
        /// Double Sharp
        /// </summary>
        [Description("##")]
        DoubleSharp
    }
}
