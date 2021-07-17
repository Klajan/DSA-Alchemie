using System;
using System.Text.RegularExpressions;

namespace Alchemie.Models.Types
{
    public readonly struct Haltbarkeit : IEquatable<Haltbarkeit>
    {
        // Regex to match a string like 2W6+6 Monate
        private static readonly Regex _parserRegex = new(@"^(?'first'.*?)(?:(?'count'\d+)?[wd](?'sides'\d+)(?'add'[+-]?\d+)?)(?'second'(?:.*?(?'time'(?>tag\(?e?\)?)|(?>woche\(?n?\)?)|(?>monat\(?e?\)?)|(?>jahr\(?e?\)?)).*?)|.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private readonly string _a;
        private readonly string _b;
        public readonly string TimeUnit { get; }
        public readonly string FullText { get; }
        public readonly ComplexDice Dice { get; }
        public readonly bool IsParsed { get; }

        public Haltbarkeit(string input)
        {
            IsParsed = false;
            _a = _b = TimeUnit = string.Empty;
            int count = 1;
            int sides = 1;
            int add = 0;
            FullText = input;
            Match m = _parserRegex.Match(input);
            if (m.Success)
            {
                Group group;
                if (m.Groups.TryGetValue("sides", out group) && group.Success)
                {
                    IsParsed = int.TryParse(group.Value, out sides);
                }
                if (m.Groups.TryGetValue("count", out group) && group.Success)
                {
                    IsParsed &= int.TryParse(group.Value, out count);
                }
                if (m.Groups.TryGetValue("add", out group) && group.Success)
                {
                    IsParsed &= int.TryParse(group.Value, out add);
                }
                if (m.Groups.TryGetValue("first", out group) && group.Success)
                {
                    _a = group.Value;
                }
                if (m.Groups.TryGetValue("second", out group) && group.Success)
                {
                    _b = group.Value;
                }
                if (m.Groups.TryGetValue("time", out group) && group.Success)
                {
                    TimeUnit = Regex.Replace(group.Value, @"\W", String.Empty);
                }
            }
            if (IsParsed) { Dice = new ComplexDice(1, sides, count, add); }
            else { Dice = new ComplexDice(); }
        }

        public Haltbarkeit(ComplexDice dice, string timeUnit)
        {
            _a = _b = FullText = string.Empty;
            TimeUnit = timeUnit;
            Dice = dice;
            FullText = string.Concat(dice.ToString(), ' ', timeUnit);
            IsParsed = true;
        }

        public int MaxValue => IsParsed ? Dice.Max : int.MaxValue;
        public int MinValue => IsParsed ? Dice.Min : int.MinValue;

        public int GetValue()
        {
            return IsParsed ? Dice.Roll() : 0;
        }

        public string GetFullHaltbarkeitStr()
        {
            return GetFullHaltbarkeitStr(GetValue());
        }

        public string GetFullHaltbarkeitStr(int num)
        {
            return IsParsed && num >= 0 ? string.Concat(_a, num, _b) : string.Empty;
        }

        public string GetHaltbarkeitStr()
        {
            return GetHaltbarkeitStr(GetValue());
        }

        public string GetHaltbarkeitStr(int num)
        {
            if (IsParsed && num >= 0)
            {
                if (TimeUnit.Length != 0)
                {
                    return string.Concat(num, ' ', num != 1 ? TimeUnit : TimeUnit[0..^1]);
                }
                return string.Concat(_a, num, _b);
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return FullText;
        }

        #region IEquatable

        public bool Equals(Haltbarkeit other)
        {
            return FullText == other.FullText;
        }

        public override bool Equals(object obj)
        {
            return obj is Haltbarkeit haltbarkeit &&
                   Equals(haltbarkeit);
        }

        public override int GetHashCode()
        {
            return FullText.GetHashCode(StringComparison.Ordinal);
        }

        public static bool operator ==(Haltbarkeit left, Haltbarkeit right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Haltbarkeit left, Haltbarkeit right)
        {
            return !(left == right);
        }

        #endregion IEquatable
    }
}