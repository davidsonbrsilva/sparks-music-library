using System.ComponentModel;

namespace SparksMusic.Library.Enum
{
    public enum Accident
    {
        [Description("")]
        None,

        [Description("bb")]
        DoubleFlat,

        [Description("b")]
        Flat,

        [Description("#")]
        Sharp,

        [Description("##")]
        DoubleSharp
    }
}
