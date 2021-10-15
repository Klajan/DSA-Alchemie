using System;

namespace Alchemie.Models.Types
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
}