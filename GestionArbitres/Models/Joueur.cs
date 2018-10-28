using GalaSoft.MvvmLight;
using BGayet.GIA.Utils;

namespace BGayet.GIA.Models
{
    public class Joueur: ObservableObject
    {
        private StatutJoueur _statut;
        private bool _estForfait;

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

        public string Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Classement { get; set; }
    }

    public enum StatutJoueur
    {
        [StringValue("L")] Libre = 0,
        [StringValue("O")] Occupe = 1,
        [StringValue("A")] Absent = 2,
    }
}