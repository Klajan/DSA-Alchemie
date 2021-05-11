using System;

namespace Alchemie.Models.Types
{
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

}
