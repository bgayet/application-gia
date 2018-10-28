using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BGayet.GIA.Models
{
    [Table("PARAM_TABLEAU_LISTES")]
    public class ParamTableauListes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ParamTableau))]
        public int IdParamTableau { get; set; }

        public int Classement { get; set; }
        public int Nombre { get; set; }
    }
}