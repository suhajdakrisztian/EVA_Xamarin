using System;

namespace MaciLaci_Xamarin.ViewModel
{
    public class GameField : ViewModelBase
    {

        private bool _isEmpty;
        private bool _isMaci;
        private bool _isHunter;
        private bool _isBasket;
        private bool _isTree;
        private bool _isLocked = false;

        public bool IsLocked
        {
            get => _isLocked;
            set => _isLocked = value;
        }
        
        public Int32 Number { get; set; }

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }
        public bool IsMaci
        {
            get => _isMaci;
            set
            {
                _isMaci = value;
                OnPropertyChanged();
            }
        }

        public bool IsHunter
        {
            get => _isHunter;
            set
            {
                _isHunter = value;
                OnPropertyChanged();
            }
        }
        public bool IsBasket
        {
            get => _isBasket;
            set
            {
                _isBasket = value;
                OnPropertyChanged();
            }
        }

        public bool IsTree
        {
            get => _isTree;
            set
            {
                _isTree = value;
                OnPropertyChanged();
            }
        }




        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }
    }
}
