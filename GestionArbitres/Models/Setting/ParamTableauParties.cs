using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BGayet.GIA.Models
{
    [Table("PARAM_TABLEAU_PARTIES")]
    public class ParamTableauParties
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ParamTableau))]
        public int IdParamTableau { get; set; }

        public int NumPartie { get; set; }
        public int NumPhase { get; set; }
        public int NumGroupeArbitre { get; set; }
        public int NumPartieVainqueur { get; set; }
        public int NumPartiePerdant { get; set; }
        public int Position { get; set; }
    }
}