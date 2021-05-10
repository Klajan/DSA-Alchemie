using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alchemie.Models
{
    public struct Probe : IEquatable<Probe>
    {
        public Probe(int brauen, int analyse)
        {
            BrauenMod = brauen;
            AnalyseMod = analyse;
        }

        public int BrauenMod { get; private set; }
        public int AnalyseMod { get; private set; }

        #region IEquatable

        public override bool Equals(object obj)
        {
            return obj is Probe probe && Equals(probe);
        }

        public bool Equals(Probe other)
        {
            return BrauenMod == other.BrauenMod &&
                   AnalyseMod == other.AnalyseMod;
        }

        public static bool operator ==(Probe left, Probe right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Probe left, Probe right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BrauenMod, AnalyseMod);
        }

        #endregion IEquatable
    }

    public struct Beschaffung : IEquatable<Beschaffung>
    {
        public Beschaffung(string preis, string verbreitung)
        {
            Preis = preis;
            Verbreitung = verbreitung;
        }

        public string Preis { get; private set; }
        public string Verbreitung { get; private set; }

        #region IEquatable

        public override bool Equals(object obj)
        {
            return obj is Beschaffung beschaffung && Equals(beschaffung);
        }

        public bool Equals(Beschaffung other)
        {
            return this.Preis == other.Preis &&
                   this.Verbreitung == other.Verbreitung &&
                   this.Preis == other.Preis &&
                   this.Verbreitung == other.Verbreitung;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Preis, Verbreitung, Preis, Verbreitung);
        }

        public static bool operator ==(Beschaffung left, Beschaffung right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Beschaffung left, Beschaffung right)
        {
            return !(left == right);
        }

        #endregion IEquatable
    }

    public readonly struct Labor : IEquatable<Labor>
    {
        public Labor(string labor)
        {
            switch (labor)
            {
                case "0":
                case "archaisches Labor":
                    ID = LaborID.ArchaischesLabor;
                    Name = "archaisches Labor";
                    break;

                case "1":
                case "Hexenküche":
                    ID = LaborID.Hexenküche;
                    Name = "Hexenküche";
                    break;

                case "2":
                case "Alchimistenlabor":
                    ID = LaborID.Alchemielabor;
                    Name = "Alchimistenlabor";
                    break;

                default:
                    ID = LaborID.ArchaischesLabor;
                    Name = labor;
                    break;
            }
        }

        public Labor(LaborID labor) : this(((int)labor).ToString()) { }

        public readonly string Name { get; }
        public readonly LaborID ID { get; }

        public LaborID ToLaborID() => ID;
        public override string ToString() => Name;

        public static implicit operator LaborID(Labor labor) => labor.ID; 
        public static implicit operator string(Labor labor) => labor.Name;

        #region IEquatable

        public override bool Equals(object obj)
        {
            return obj is Labor labor && Equals(labor);
        }

        public bool Equals(Labor other)
        {
            return Name == other.Name &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ID);
        }

        public static bool operator ==(Labor left, Labor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Labor left, Labor right)
        {
            return !(left == right);
        }

        #endregion IEquatable
    }

    public readonly struct Wirkung : IEquatable<Wirkung>
    {
        public Wirkung(string[] init)
        {
            M = A = B = C = D = E = F = String.Empty;

            if (init == null) return;

            for (int i = 0; i < 7 || i < init.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        M = init[i];
                        break;

                    case 1:
                        A = init[i];
                        break;

                    case 2:
                        B = init[i];
                        break;

                    case 3:
                        C = init[i];
                        break;

                    case 4:
                        D = init[i];
                        break;

                    case 5:
                        E = init[i];
                        break;

                    case 6:
                        F = init[i];
                        break;
                }
            }
        }

        public Wirkung(string m, string a, string b, string c, string d, string e, string f)
        {
            M = m; A = a; B = b; C = c; D = d; E = e; F = f;
        }

        public readonly string M { get; }
        public readonly string A { get; }
        public readonly string B { get; }
        public readonly string C { get; }
        public readonly string D { get; }
        public readonly string E { get; }
        public readonly string F { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1043:Use Integral Or String Argument For Indexers", Justification = "Intended char Indexer")]
        public string this[char index]
        {
            get
            {
                return index switch
                {
                    'M' => M,
                    'A' => A,
                    'B' => B,
                    'C' => C,
                    'D' => D,
                    'E' => E,
                    'F' => F,
                    _ => String.Empty,
                };
            }
        }

        public string this[Quality index]
        {
            get
            {
                return index switch
                {
                    Quality.M => M,
                    Quality.A => A,
                    Quality.B => B,
                    Quality.C => C,
                    Quality.D => D,
                    Quality.E => E,
                    Quality.F => F,
                    _ => String.Empty,
                };
            }
        }

        #region IEquatable

        public bool Equals(Wirkung other)
        {
            return M == other.M &&
            A == other.A &&
            B == other.B &&
            C == other.C &&
            D == other.D &&
            E == other.E &&
            F == other.F;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(M, A, B, C, D, E, F);
        }

        public static bool operator ==(Wirkung left, Wirkung right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Wirkung left, Wirkung right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Wirkung wirkung && Equals(wirkung);
        }

        #endregion IEquatable
    }

    public readonly struct Haltbarkeit : IEquatable<Haltbarkeit>
    {
        // Regex to match a string like 2W6+6 Monate
        private static readonly Regex _regex = new(@"^(?'first'.*?)(?:(?'count'\d+)?[wd](?'sides'\d+)(?'add'[+-]?\d+)?)(?'second'(?:.*?(?'time'(?>tag\(?e?\)?)|(?>woche\(?n?\)?)|(?>monat\(?e?\)?)|(?>jahr\(?e?\)?)).*?)|.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private readonly string _a;
        private readonly string _b;
        private readonly string _timeUnit;
        public readonly string FullText { get; }
        public readonly ComplexDice Dice { get; }
        private readonly bool _parsed;

        public Haltbarkeit(string input)
        {
            _parsed = false;
            _a = _b = _timeUnit = String.Empty;
            int count = 1;
            int sides = 1;
            int add = 0;
            FullText = input;
            Match m = _regex.Match(input);
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
                    _timeUnit = Regex.Replace(group.Value, @"\W", String.Empty);
                }
            }
            if (_parsed) { Dice = new ComplexDice(1, sides, count, add); }
            else { Dice = new ComplexDice(); }
        }

        public Haltbarkeit(ComplexDice dice, string timeUnit)
        {
            _a = _b = FullText = String.Empty;
            _timeUnit = timeUnit;
            Dice = dice;
            FullText = String.Concat(dice.ToString(), ' ', timeUnit);
            _parsed = true;
        }

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
                String.Concat(num, ' ' , _timeUnit.Length == 0 ? _b :
                num != 1 ? _timeUnit :
                _timeUnit[0..^1]) :
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
