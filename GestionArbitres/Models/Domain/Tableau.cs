using System.Collections.Generic;

namespace BGayet.GIA.Models
{
    public class Tableau
    {
        public List<Partie> Parties { get; set; }
        public List<Joueur> Joueurs { get; set; }
        public List<Table> Tables { get; set; }
    }
}