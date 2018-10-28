using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BGayet.GIA.Models
{
    [Table("PARAM_TABLEAU_JOUEURS")]
    public class ParamTableauJoueurs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ParamTableau))]
        public int IdParamTableau { get; set; }

        public int NumLigneFichier { get; set; }
        public int NumPartieTableau { get; set; }
        public int? ClassementJoueur1 { get; set; }
        public int? ClassementJoueur2 { get; set; }
    }
}