using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private readonly bool _parsed;

        public Haltbarkeit(string input)
        {
            _parsed = false;
            _a = _b = TimeUnit = String.Empty;
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
                    _parsed = Int32.TryParse(group.Value, out sides);
                }
                if (m.Groups.TryGetValue("count", out group) && group.Success)
                {
                    _parsed &= Int32.TryParse(group.Value, out count);
                }
                if (m.Groups.TryGetValue("add", out group) && group.Success)
                {
                    _parsed &= Int32.TryParse(group.Value, out add);
                }
                if (m.Groups.TryGetValue("first", out group) && group.Success)
                {
                    _a = group.Value;
                }
                if (m.Groups.TryGetValue("second", out group) && group.Success)
                {
                    _b = group.Value;
                }
                if(m.Groups.TryGetValue("time", out group) && group.Success)
                {
                    TimeUnit = Regex.Replace(group.Value, @"\W", String.Empty);
                }
            }
            if (_parsed) { Dice = new ComplexDice(1, sides, count, add); }
            else { Dice = new ComplexDice(); }
        }

        public Haltbarkeit(ComplexDice dice, string timeUnit)
        {
            _a = _b = FullText = String.Empty;
            TimeUnit = timeUnit;
            Dice = dice;
            FullText = String.Concat(dice.ToString(), ' ', timeUnit);
            _parsed = true;
        }

        public int MaxValue => _parsed ? Dice.Max : Int32.MaxValue;
        public int MinValue => _parsed ? Dice.Min : Int32.MinValue;

        public int GetValue()
        {
            return _parsed ? Dice.Roll() : 0;
        }

        public string GetValueStringRandom()
        {
            return GetValueString(Dice.Roll());
        }

        public string GetValueString(int num)
        {
            return _parsed && num >= 0 ? String.Concat(_a, num, _b) : String.Empty;
        }

        public string GetTimeSpanString(int num)
        {
            return _parsed && num >= 0 ?
                String.Concat(num, ' ' , TimeUnit.Length == 0 ? _b :
                num != 1 ? TimeUnit :
                TimeUnit[0..^1]) :
                String.Empty;
        }

        public bool IsParsed() => _parsed;

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
