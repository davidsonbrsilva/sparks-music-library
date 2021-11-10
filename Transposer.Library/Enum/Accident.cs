using System.ComponentModel;

namespace Transposer.Library.Enum
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
