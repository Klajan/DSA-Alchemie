using System;
using System.Collections.Generic;
using Alchemie.Core;

namespace Alchemie.Models
{
    public readonly struct Dice : IEquatable<Dice>
    {
        private static readonly Random rnd = new();

        public readonly int Min { get; }
        public readonly int Max { get; }

        public Dice(int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min), "min cannot be larger than max");
            Min = min; Max = max;
        }

        public int Roll()
        {
            return rnd.Next(Min, Max + 1);
        }

        public void Roll(ref int[] array)
        {
            if (array == null) return;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Roll();
            }
        }

        public int[] Roll(int amount)
        {
            int[] arr = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                arr[i] = Roll();
            }
            return arr;
        }

        public void Roll(ref IList<int> vs)
        {
            if (vs == null) return;
            for(int i = 0; i < vs.Count; i++)
            {
                vs[i] = Roll();
            }
        }

        public override string ToString()
        {
            return String.Concat("W", Max);
        }

        #region IEquatable

        public bool Equals(Dice other)
        {
            return Min == other.Min && Max == other.Max;
        }

        public override bool Equals(object obj)
        {
            return obj is Dice dice && Equals(dice);
        }

        public override int GetHashCode()
        {
            return ((((long)Min) << 32) | (Max & 0xffffffffL)).GetHashCode();
        }

        public static bool operator ==(Dice left, Dice right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Dice left, Dice right)
        {
            return !(left == right);
        }

        #endregion IEquatable
    }

    public readonly struct ComplexDice : IEquatable<ComplexDice>
    {
        public readonly Dice Dice { get; }
        public readonly int Count { get; }
        public readonly int Add { get; }

        public int MinRoll { get => Dice.Min * Count + Add; }
        public int MaxRoll { get => Dice.Max * Count + Add; }

        public ComplexDice(int min, int max, int count = 1, int add = 0)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "count cannot be less then 1");
            Dice = new Dice(min, max);
            Count = count;
            Add = add;
        }

        public int Roll()
        {
            int res = Add;
            for(int i = 0; i < Count; i++)
            {
                res += Dice.Roll();
            }
            return res;
        }

        public int Roll(out int[] values)
        {
            values = new int[Count];
            int res = Add;
            for (int i = 0; i < Count; i++)
            {
                int v = Dice.Roll();
                res += v;
                values[i] = v;
            }
            return res;
        }

        public int Roll(out IList<int> values)
        {
            int res = Add;
            values = new List<int>(Count);
            for (int i = 0; i < Count; i++)
            {
                int t = Dice.Roll();
                res += t;
                values.Add(t);
            }
            return res;
        }

        public override string ToString()
        {
            return String.Concat(Count, Dice.ToString(), Add != 0 ? Add.ToString("+#;-#") : String.Empty);
        }

        #region IEquatable

        public bool Equals(ComplexDice other)
        {
            return Dice == other.Dice && Count == other.Count && Add == other.Add;
        }

        public override bool Equals(object obj)
        {
            return obj is ComplexDice dice && Equals(dice);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dice, Count, Add);
        }

        public static bool operator ==(ComplexDice left, ComplexDice right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComplexDice left, ComplexDice right)
        {
            return !(left == right);
        }

        #endregion IEquatable
    }
}