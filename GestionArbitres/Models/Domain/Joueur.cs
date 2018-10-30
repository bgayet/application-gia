using GalaSoft.MvvmLight;
using BGayet.GIA.Utils;

namespace BGayet.GIA.Models
{
    public class Joueur: ObservableObject
    {
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

        public string Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}