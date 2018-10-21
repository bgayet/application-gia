using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace GestionArbitres.Models
{
    public class ParamTableau
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Libelle { get; set; }
        public int Hauteur { get; set; }
        public int Largeur { get; set; }
        [OneToMany]
        public List<ParamTableauParties> ParamTableauParties { get; set; }
    }
}