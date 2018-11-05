using GalaSoft.MvvmLight;

namespace BGayet.GIA.Models
{
    public class Groupe : ObservableObject
    {
        private int _numero;
        private int _classement;

        public int Numero
        {
            get => _numero;
            set => Set(ref _numero, value);
        }

        public int Classement
        {
            get => _classement;
            set => Set(ref _classement, value);
        }
    }
}