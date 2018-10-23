using System.Collections.Generic;

namespace BGayet.GIA.Models
{
    public class Tableau
    {
        public Partie[] Parties { get; set; }
        public List<Joueur> Joueurs { get; set; }
        public List<Table> Tables { get; set; }
    }
}