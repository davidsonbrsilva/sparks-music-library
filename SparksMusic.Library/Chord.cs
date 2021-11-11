using System;
using System.Text.RegularExpressions;
using SparksMusic.Library.Enum;
using SparksMusic.Library.Exceptions;
using SparksMusic.Library.Extensions;
using SparksMusic.Library.Utils;

namespace SparksMusic.Library
{
    /// <summary>
    /// Chord class
    /// </summary>
    public class Chord : IEquatable<Chord>
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
        /// Creates a chord object from a string. Considers only the first match.
        /// </summary>
        /// <param name="chord">The chord</param>
        public Chord(string chord)
        {
            var regex = new Regex(GetChordPattern());
            var match = regex.Match(chord);

            if (!match.Success || match.Value != chord)
                throw new NotAChordException("The string provided does not match a valid chord.");

            const int noteLetterGroup = 1;
            const int accidentGroup = 2;
            const int diminutiveOrAugmentedGroup = 3;
            const int tonalityGroup = 4;
            const int firstComplementGroup = 5;
            const int secondComplementGroup = 6;
            const int inversionLetterGroup = 11;
            const int inversionAccidentGroup = 12;

            Note = GetNote(match.Groups[noteLetterGroup].Value, match.Groups[accidentGroup].Value);
            Tonality = GetTonality(match.Groups[diminutiveOrAugmentedGroup].Value, match.Groups[tonalityGroup].Value);
            Complement = GetComplement(Tonality, $"{match.Groups[firstComplementGroup].Value}{match.Groups[secondComplementGroup].Value}");
            Inversion = GetInversion(match.Groups[inversionLetterGroup].Value, match.Groups[inversionAccidentGroup].Value);
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
            Console.WriteLine(Inversion);
            string inversionString = Inversion is not null ? $"/{Inversion}" : "";
            return $"{Note}{Tonality.GetDescription()}{Complement}{inversionString}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as Chord);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Note, Tonality, Complement, Inversion);
        }

        private static string GetChordPattern()
        {
            const string noteLetter = @"([A-G]{1})";
            const string accident = @"(##?|bb?)?";
            const string tonality = @"(m|sus2|sus4)?";
            const string interval = @"(2|4|6|7M|7|9|11|13)?";
            const string incrementInterval = @"(b2|2|4|4#|b5|5|#5|6|7|7M|b9|9|11|#11|13)";
            
            string note = $@"{noteLetter}{accident}";
            string complement = $@"(\+|°|{tonality}{interval}(\({incrementInterval}(,{incrementInterval})*\))?)";
            string inversion = $@"(\/{noteLetter}{accident})?";

            return $"{note}{complement}{inversion}";
        }

        private static Note GetNote(string noteLetterValue, string accidentValue)
        {
            NoteLetter noteLetter = EnumUtils.Parse<NoteLetter>(noteLetterValue);
            Accident accident = EnumUtils.Parse<Accident>(accidentValue);
            return new Note(noteLetter, accident);
        }

        private static Tonality GetTonality(string diminutiveOrAugmentedValue, string tonality)
        {
            if (diminutiveOrAugmentedValue == Tonality.Augmented.GetDescription())
            {
                return Tonality.Augmented;
            }
            else if (diminutiveOrAugmentedValue == Tonality.Diminuted.GetDescription())
            {
                return Tonality.Diminuted;
            }
            else if (diminutiveOrAugmentedValue == Tonality.HalfDiminuted.GetDescription())
            {
                return Tonality.HalfDiminuted;
            }
            else
            {
                return EnumUtils.Parse<Tonality>(tonality);
            }
        }

        private static string GetComplement(Tonality tonality, string text)
        {
            if (tonality == Tonality.Augmented || tonality == Tonality.Diminuted || tonality == Tonality.HalfDiminuted)
            {
                return "";
            }
            else
            {
                return text;
            }
        }

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
    }
}
