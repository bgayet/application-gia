using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class ListViewInscritsItemModel: ObservableObject
    {
        private int _compteurArbitre;

        public int CompteurArbitre
        {
            get => _compteurArbitre;
            set => Set(ref _compteurArbitre, value);
        }

        public Joueur Joueur { get; set; }
        public int Classement { get; set; }
        public int NumGroupeArbitre { get; set; }
    }
}