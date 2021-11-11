using System.ComponentModel;

namespace SparksMusic.Library.Enum
{
    public enum Tonality
    {
        [Description("")]
        Major,

        [Description("m")]
        Minor,

        [Description("+")]
        Augmented,

        [Description("°")]
        Diminuted,

        [Description("m7(b5)")]
        HalfDiminuted,

        [Description("sus2")]
        Sus2,

        [Description("sus4")]
        Sus4
    }
}
