using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class JoueurModel: ObservableObject
    {
        private int _compteurArbitre;
        private StatutJoueurModel _statut;

        public int CompteurArbitre
        {
            get => _compteurArbitre;
            set => Set(ref _compteurArbitre, value);
        }

        public StatutJoueurModel Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }

        public Joueur Joueur { get; set; }
        public int Classement { get; set; }
        public int NumGroupeArbitre { get; set; }
    }

    public enum StatutJoueurModel
    {
        Aucun = 0,
        Libre = 1,
        Occupe = 2
    }
}