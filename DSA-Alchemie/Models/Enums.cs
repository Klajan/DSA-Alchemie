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

    public enum Qualität
    {
        M = 'M',
        A = 'A',
        B = 'B',
        C = 'C',
        D = 'D',
        E = 'E',
        F = 'F',
        None = 0
    }
}