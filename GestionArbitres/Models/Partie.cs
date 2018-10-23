using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class Partie : ObservableObject
    {
        private Joueur _joueur1;
        private Joueur _joueur2;
        private Joueur _arbitre;
        private Joueur _vainqueur;
        private StatutPartie _statut;

        public Joueur Joueur1
        {
            get => _joueur1;
            set => Set(ref _joueur1, value);
        }

        public Joueur Joueur2
        {
            get => _joueur2;
            set => Set(ref _joueur2, value);
        }

        public Joueur Arbitre
        {
            get => _arbitre;
            set => Set(ref _arbitre, value);
        }

        public Joueur Vainqueur
        {
            get => _vainqueur;
            set => Set(ref _vainqueur, value);
        }

        public StatutPartie Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }

        public int Numero { get; set; }
        public int NumeroPhase { get; set; }
        public int NumeroTable { get; set; }
        public int NumeroTableau { get; set; }
        public Partie PartieVainqueur { get; set; }
        public Partie PartiePerdant { get; set; }
        public int Position { get; set; }
    }

    public enum StatutPartie
    {
        ALancer = 1,
        EnAttente = 2,
        EnCours = 3,
        Terminee = 4,
    }

}