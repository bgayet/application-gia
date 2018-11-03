using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BGayet.GIA.Models
{
    public class Tableau
    {
        public List<Partie> Parties { get; set; }
        public ObservableCollection<Joueur> Joueurs { get; set; }
        public List<Table> Tables { get; set; }
    }
}