using System;
using System.Text.RegularExpressions;
using SparksMusic.Library.Enum;
using SparksMusic.Library.Extensions;
using SparksMusic.Library.Utils;

namespace SparksMusic.Library
{
    /// <summary>
    /// Classe de acorde.
    /// </summary>
    public class Chord : IEquatable<Chord>
    {
        public Note Note { get; }
        public Tonality Tonality { get; }
        public string Complement { get; }
        public Note Inversion { get; }

        /// <summary>
        /// Cria um objeto de acorde a partir de uma string. Considera somente o primeiro match.
        /// </summary>
        /// <param name="chord"></param>
        public Chord(string chord)
        {
            var regex = new Regex(GetChordPattern());
            var match = regex.Match(chord);

            if (!match.Success)
                throw new NotAChordException("A cadeia de caracteres fornecida não corresponde a um acorde válido.");

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

            if (Tonality == Tonality.Augmented || Tonality == Tonality.Diminuted || Tonality == Tonality.HalfDiminuted)
                Complement = "";
            else
                Complement = match.Groups[firstComplementGroup].Value + match.Groups[secondComplementGroup].Value;

            Inversion = GetInversion(match.Groups[inversionLetterGroup].Value, match.Groups[inversionAccidentGroup].Value);
        }

        /// <summary>
        /// Cria um objeto de acorde.
        /// </summary>
        /// <param name="note">A nota do acorde</param>
        /// <param name="tonality">A tonalidade do acorde.</param>
        /// <param name="complement">O complemento do acorde.</param>
        /// <param name="inversion">A inversão do acorde.</param>
        public Chord(Note note, Tonality tonality = Tonality.Major, string complement = "", Note inversion = null)
        {
            Note = note;
            Tonality = tonality;
            Complement = complement;
            Inversion = inversion;
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
            return HashCode.Combine(Note,Tonality,Complement,Inversion);
        }

        public bool Equals(Chord other)
        {
            return other != null && ToString().Equals(other.ToString());
        }

        public static Chord operator ++(Chord chord)
        {
            return Transposer.TransposeUp(chord, 1);
        }

        public static Chord operator --(Chord chord)
        {
            return Transposer.TransposeDown(chord, 1);
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
