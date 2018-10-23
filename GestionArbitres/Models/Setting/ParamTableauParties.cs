using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BGayet.GIA.Models
{
    public class ParamTableauParties
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(ParamTableau))]
        public int IdTableau { get; set; }
        public int NumPartie { get; set; }
        public int NumPhase { get; set; }
        public int NumPartieVainqueur { get; set; }
        public int NumPartiePerdant { get; set; }
        public int Position { get; set; }
    }
}