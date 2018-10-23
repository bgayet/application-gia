using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class Table : ObservableObject
    {
        private StatutTable _statut;

        public int Numero { get; set; }
        public int NumeroTableau { get; set; }
        public StatutTable Statut
        {
            get => _statut;
            set => Set(ref _statut, value);
        }
    }

    public enum StatutTable
    {
        Libre = 1,
        Occupe = 2,
    }
}