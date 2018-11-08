using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace BGayet.GIA.Models
{
    public class Tableau: ObservableObject
    {
        private ObservableCollection<Partie> _parties;
        private ObservableCollection<Joueur> _joueurs;
        private ObservableCollection<Table> _tables;

        public ObservableCollection<Partie> Parties
        {
            get => _parties;
            set => Set(ref _parties, value);
        }

        public ObservableCollection<Joueur> Joueurs
        {
            get => _joueurs;
            set => Set(ref _joueurs, value);
        }

        public ObservableCollection<Table> Tables
        {
            get => _tables;
            set => Set(ref _tables, value);
        }
    }
}