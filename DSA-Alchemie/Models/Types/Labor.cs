using System;

namespace Alchemie.Models.Types
{
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

}
