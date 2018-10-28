using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace BGayet.GIA.Models
{
    [Table("PARAM_TABLEAU")]
    public class ParamTableau
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany]
        public List<ParamTableauParties> ParamTableauParties { get; set; }

        [OneToMany]
        public List<ParamTableauJoueurs> ParamTableauJoueurs { get; set; }

        [OneToMany]
        public List<ParamTableauListes> ParamTableauListes { get; set; }

        public string Libelle { get; set; }
        public int Hauteur { get; set; }
        public int Largeur { get; set; }
    }
}