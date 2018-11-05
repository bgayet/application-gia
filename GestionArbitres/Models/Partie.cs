using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class Partie : ObservableObject
    {
        private Joueur _joueur1;
        private Joueur _joueur2;
        private Joueur _arbitre;
        private Table _table;
        private ResultatPartie _resultat;
        private StatutPartie _statut;

        public Joueur Joueur1
        {
            get => _joueur1;
            set
            {
                value.Partie = this;
                Set(ref _joueur1, value);
            }
        }

        public Joueur Joueur2
        {
            get => _joueur2;
            set
            {
                value.Partie = this;
                Set(ref _joueur2, value);
            }
        }

        public Joueur Arbitre
        {
            get => _arbitre;
            set
            {
                value.Partie = this;
                Set(ref _arbitre, value);
            }
        }

        public Table Table
        {
            get => _table;
            set => Set(ref _table, value);
        }

        public ResultatPartie Resultat
        {
            get => _resultat;
            set => Set(ref _resultat, value);
        }

        public StatutPartie Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }

        public int Numero { get; set; }
    }

    public enum ResultatPartie
    {
        Joueur1Vainqueur = 0,
        Joueur2Vainqueur = 1
    }

    public enum StatutPartie
    {
        ALancer = 0,
        EnAttente = 1,
        EnAttenteArbitre = 2,
        EnCours = 3,
        Terminee = 4,
    }
}