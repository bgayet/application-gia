using GalaSoft.MvvmLight;
using GestionArbitres.Utils;

namespace GestionArbitres.Model
{
    public class Joueur: ObservableObject
    {
        private StatutJoueur _statut;

        public string Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public StatutJoueur Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }
    }

    public enum StatutJoueur
    {
        [StringValue("L")] Libre = 1,
        [StringValue("O")] Occupe = 2,
        [StringValue("A")] Absent = 3,
        [StringValue("FL")] ForfaitLibre = 4,
        [StringValue("FO")] ForfaitOccupe = 5,
        [StringValue("FA")] ForfaitAbsent = 6,
    }

}