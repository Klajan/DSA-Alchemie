using System;

namespace Alchemie.Models
{
    public enum LaborID
    {
        ArchaischesLabor = 0,
        Hexenküche = 1,
        Alchemielabor = 2,
    }

    public enum LaborQL
    {
        Fehlend = +3,
        Normal = 0,
        Gut = -3,
        SehrGut = -7
    }

    public enum Subsitution
    {
        Optimierend = -3,
        Gleichwertig = 0,
        Sinnvoll = +3,
        Möglich = +6,
        Unsinnig = UInt16.MaxValue
    }

    public enum Quality
    {
        None = -1,
        M = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6
    }
}