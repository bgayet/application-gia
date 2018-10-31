using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class Joueur: ObservableObject
    {
        private int _compteurArbitre;
        private StatutJoueur _statut;
        private bool _estForfait;
        private bool _estAbsent;

        public bool EstAbsent
        {
            get => _estAbsent;
            set => Set(ref _estAbsent, value);
        }

        public bool EstForfait
        {
            get => _estForfait;
            set => Set(ref _estForfait, value);
        }

        public StatutJoueur Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }

        public int CompteurArbitre
        {
            get => _compteurArbitre;
            set => Set(ref _compteurArbitre, value);
        }

        public string Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Classement { get; set; }
        public int NumGroupeArbitre { get; set; }
    }

    public enum StatutJoueur
    {
        Libre = 0,
        Occupe = 1
    }
}