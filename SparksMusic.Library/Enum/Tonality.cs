using System.ComponentModel;

namespace SparksMusic.Library
{
    /// <summary>
    /// Tonality enum
    /// </summary>
    public enum Tonality
    {
        /// <summary>
        /// Major
        /// </summary>
        [Description("")]
        Major,

        /// <summary>
        /// Minor
        /// </summary>
        [Description("m")]
        Minor,

        /// <summary>
        /// Augmented
        /// </summary>
        [Description("+")]
        Augmented,

        /// <summary>
        /// Diminished
        /// </summary>
        [Description("\u00B0")]
        Diminished,

        /// <summary>
        /// Half-Diminished
        /// </summary>
        [Description("m7(b5)")]
        HalfDiminished,

        /// <summary>
        /// Suspended 2
        /// </summary>
        [Description("sus2")]
        Sus2,

        /// <summary>
        /// Suspended 4
        /// </summary>
        [Description("sus4")]
        Sus4
    }
}
