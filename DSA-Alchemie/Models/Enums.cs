namespace Alchemie.Models
{
    public enum LaborID : int
    {
        ArchaischesLabor = 0,
        Hexenküche = 1,
        Alchemielabor = 2,
    }

    public enum LaborQL : int
    {
        Fehlend = +3,
        Normal = 0,
        Gut = -3,
        SehrGut = -7
    }

    public enum Subsitution : int
    {
        Optimierend = -3,
        Gleichwertig = 0,
        Sinnvoll = +3,
        Möglich = +6,
        Unsinnig = int.MaxValue
    }
}