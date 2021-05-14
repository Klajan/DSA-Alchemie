using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alchemie.Models.Types;
using Alchemie.Models;
using Alchemie.Core;

namespace Alchemie.UI.Views
{
    public static class Templates
    {
        public static readonly Tuple<LaborQL, string>[] LabQualityList = new Tuple<LaborQL, string>[]
        {
            Tuple.Create(LaborQL.Fehlend, "(+3) Fehlende/beschädigte Gerätschaften"),
            Tuple.Create(LaborQL.Normal, "(+0) Normales Labor"),
            Tuple.Create(LaborQL.Gut, "(-3) Hochwertiges Labor"),
            Tuple.Create(LaborQL.SehrGut, "(-7) Außergewöhnlich hochwertiges Labor")
        };

        public static readonly Tuple<LaborID, string>[] LabLevelList = new Tuple<LaborID, string>[]
        {
            Tuple.Create(LaborID.ArchaischesLabor, "archaisches Labor"),
            Tuple.Create(LaborID.Hexenküche, "Hexenküche"),
            Tuple.Create(LaborID.Alchemielabor, "Alchimistenlabor")
        };

        public static readonly Tuple<Subsitution, string>[] SubstitutionList = new Tuple<Subsitution, string>[]
        {
            Tuple.Create(Subsitution.Optimierend, "(-3) Optimierende Substitution" ),
            Tuple.Create(Subsitution.Gleichwertig, "(+0) Gleichwertige Substitution"),
            Tuple.Create(Subsitution.Sinnvoll, "(+3) Sinnvolle Substitution"),
            Tuple.Create(Subsitution.Möglich, "(+6) Mögliche Substitution"),
            Tuple.Create(Subsitution.Unsinnig, "Unsinnige Substitution")
        };

        public static readonly Quality[] QualityList = Enum.GetValues<Quality>();

        public static readonly string[] TimeSpanList = new string[]
        {
            "None",
            "Tage",
            "Wochen",
            "Monate",
            "Jahre",
        };
    }
}
