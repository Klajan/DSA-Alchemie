using System;

namespace Alchemie.Models.Types
{
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
            return Preis == other.Preis &&
                   Verbreitung == other.Verbreitung &&
                   Preis == other.Preis &&
                   Verbreitung == other.Verbreitung;
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
}