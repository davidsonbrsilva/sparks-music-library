using SparksMusic.Library.Utils;
using System;
using System.Text.RegularExpressions;

namespace SparksMusic.Library;

/// <summary>
/// Chord class
/// </summary>
public partial class Chord : IEquatable<Chord>
{
    /// <summary>
    /// Chord note
    /// </summary>
    public Note Note { get; }

    /// <summary>
    /// Chord tonality
    /// </summary>
    public Tonality Tonality { get; }

    /// <summary>
    /// Chord complement
    /// </summary>
    public string Complement { get; }

    /// <summary>
    /// Chord inversion
    /// </summary>
    public Note Inversion { get; }

    /// <summary>
    /// True if chord is flat
    /// </summary>
    public bool IsFlat { get => Note.IsFlat; }

    /// <summary>
    /// True if chord is double flat
    /// </summary>
    public bool IsDoubleFlat { get => Note.IsDoubleFlat; }

    /// <summary>
    /// True if chord is flat or double flat
    /// </summary>
    public bool IsFlatOrDoubleFlat { get => IsFlat || IsDoubleFlat; }

    /// <summary>
    /// True if chord is sharp
    /// </summary>
    public bool IsSharp { get => Note.IsSharp; }

    /// <summary>
    /// True if chord is double sharp
    /// </summary>
    public bool IsDoubleSharp { get => Note.IsDoubleSharp; }

    /// <summary>
    /// True if chord is sharp or double sharp
    /// </summary>
    public bool IsSharpOrDoubleSharp { get => IsSharp || IsDoubleSharp; }

    /// <summary>
    /// Creates a chord object from a string.
    /// </summary>
    /// <param name="chord">The chord</param>
    /// <exception cref="ArgumentNullException">Thrown when chord parameter is null.</exception>
    /// <exception cref="NotAChordException">Thrown when chord parameter is not a valid chord.</exception>
    public Chord(string chord)
    {
        if (chord is null)
            throw new ArgumentNullException(nameof(chord));

        chord = chord.Trim();

        var regex = ChordRegex();
        var match = regex.Match(chord);

        if (!match.Success || match.Value != chord)
            throw new NotAChordException("The string provided does not match a valid chord");

        const string key = "key";
        const string chromatism = "chromatism";
        const string complement = "complement";
        const string tonality = "tonality";
        const string inversionKey = "inversionKey";
        const string inversionChromatism = "inversionChromatism";

        Note = GetNote(match.Groups[key].Value, match.Groups[chromatism].Value);
        Tonality = GetTonality(match.Groups[complement].Value, match.Groups[tonality].Value);
        Complement = GetComplement(Tonality, match.Groups[complement].Value);
        Inversion = GetInversion(match.Groups[inversionKey].Value, match.Groups[inversionChromatism].Value);
    }

    /// <summary>
    /// Creates a chord object.
    /// </summary>
    /// <param name="note">The chord note</param>
    /// <param name="tonality">The chord tonality</param>
    /// <param name="complement">The chord complement</param>
    /// <param name="inversion">The chord inversion</param>
    public Chord(Note note, Tonality tonality = Tonality.Major, string complement = "", Note inversion = null)
    {
        Note = note;
        Tonality = tonality;
        Complement = complement;
        Inversion = inversion;
    }

    /// <summary>
    /// Compare two chords.
    /// </summary>
    /// <param name="other">The other chord</param>
    /// <returns>True if both have same name.</returns>
    public bool Equals(Chord other)
    {
        return other != null && ToString().Equals(other.ToString());
    }

    /// <summary>
    /// Transposes up chord a semitone.
    /// </summary>
    /// <param name="chord">The chord</param>
    /// <returns>The transposed chord</returns>
    public static Chord operator ++(Chord chord)
    {
        return Transposer.TransposeUp(chord, 1);
    }

    /// <summary>
    /// Transposes down chord a semitone.
    /// </summary>
    /// <param name="chord">The chord</param>
    /// <returns>The transposed chord</returns>
    public static Chord operator --(Chord chord)
    {
        return Transposer.TransposeDown(chord, 1);
    }

    public override string ToString()
    {
        string inversionString = Inversion != null ? $"/{Inversion}" : "";
        return $"{Note}{Tonality.GetDescription()}{Complement}{inversionString}";
    }

    public override bool Equals(object obj) => base.Equals(obj as Chord);

    public override int GetHashCode() => HashCode.Combine(Note, Tonality, Complement, Inversion);

    private static Note GetNote(string noteLetterValue, string accidentValue)
    {
        NoteLetter noteLetter = EnumUtils.Parse<NoteLetter>(noteLetterValue);
        Accident accident = EnumUtils.Parse<Accident>(accidentValue);
        return new Note(noteLetter, accident);
    }

    private static Tonality GetTonality(string complement, string tonality)
    {
        try
        {
            return EnumUtils.Parse<Tonality>(complement);
        }
        catch (Exception)
        {
            try
            {
                return EnumUtils.Parse<Tonality>(tonality);
            }
            catch (Exception)
            {
                throw new Exception("Unknown tonality");
            }
        }
    }

    private static string GetComplement(Tonality tonality, string text) => tonality switch
    {
        Tonality.Augmented 
          or Tonality.Diminished 
          or Tonality.HalfDiminished 
          or Tonality.Sus2 
          or Tonality.Sus4 => "",
        Tonality.Minor => text[1..],
        _ => text
    };

    private static Note GetInversion(string noteLetterValue, string accidentValue)
    {
        try
        {
            return GetNote(noteLetterValue, accidentValue);
        }
        catch (Exception)
        {
            return null;
        }
    }

    [GeneratedRegex("^((?<key>[A-G])(?<chromatism>##?|bb?)?)(?<complement>\\+|°|((?<tonality>m?)(2|4|5|6|7M?|maj7|9M?|maj9|11|13|sus(2|4)?|add(2|3|5|6|9|11|13))?)?(\\((b2|2|4|4#|b5|5|#5|6|7M?|maj7|b9|9M?|maj9|11|#11|13|sus(2|4)?|add(2|3|5|6|9|11|13))(,(b2|2|4|4#|b5|5|#5|6|7M?|maj7|b9|9|11|#11|13|sus(2|4)|add(2|3|5|6|9|11|13)))*\\))?)(\\/(?<inversionKey>[A-G])(?<inversionChromatism>##?|bb?)?)?$", RegexOptions.Compiled)]
    private static partial Regex ChordRegex();
}
