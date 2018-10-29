using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BGayet.GIA.Models
{
    [Table("PARAM_TABLEAU_GROUPES")]
    public class ParamTableauGroupes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ParamTableau))]
        public int IdParamTableau { get; set; }

        public int NumGroupe { get; set; }
        public int ClassementJoueurs { get; set; }
        public int NombreJoueurs { get; set; }
    }
}