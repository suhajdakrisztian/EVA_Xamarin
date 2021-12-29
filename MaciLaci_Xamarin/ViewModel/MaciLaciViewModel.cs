using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows;
using MaciLaci_Xamarin.Model;

namespace MaciLaci_Xamarin.ViewModel
{
    class MaciLaciViewModel : ViewModelBase
    {
        private GameModel _model;
        private int _timeElapsed = 0;
        private string _tableSize = "10";
        private bool _gameIsRunning = true;

        public readonly List<string> TableSizeSelector = new  List<String> {"10", "11", "12"};

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand ChangeStatusCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand GameEndedCommand { get; private set; }
        public DelegateCommand TableSizeChangedCommand { get; private set; }
        public ObservableCollection<GameField> Fields { get; set; }


        public event EventHandler NewGame;
        public event EventHandler ChangeStatus;
        public event EventHandler GameEnded;

        public int TimeElapsed
        {
            get => _timeElapsed;
            set
            {
                _timeElapsed = value;
                OnPropertyChanged("TimeElapsed");
            }
        }

        public bool GameIsRunning
        {
            get => _gameIsRunning;
            set
            {
                _gameIsRunning = value;
                OnPropertyChanged("GameIsRunning");
            }
        }
        
        public string TableSize
        {
            get => _tableSize;
            set
            {
                _tableSize = value;
                OnPropertyChanged("TableSize");
            }
        }
        
        public string BasketsGathered
        {
            get => _model.getBasketGathered().ToString();

        }
        

        public MaciLaciViewModel(GameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.GameCreated += new EventHandler<EventArgs>(Model_GameCreated);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            ChangeStatusCommand = new DelegateCommand(param => OnGameStatusChanged());
            GameEndedCommand = new DelegateCommand(param => OnGameEnded());

            
            // játéktábla létrehozása
            Fields = new ObservableCollection<GameField>();
            for (Int32 i = 0; i < _model.FieldSize; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.FieldSize; j++)
                {
                    GameField temp = new GameField();
                    temp.X = i;
                    temp.Y = j;
                    temp.Number = i * _model.FieldSize + j;
                    temp.StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)));
                    Fields.Add(temp);
                }
            }

            Refresh();
            OnPropertyChanged("BasketsGathered");
        }
        private void StepGame(Int32 index)
        {
            if (Fields[index].IsLocked)
                return;

            GameField field = Fields[index];
            _model.PlayerMove(field.X, field.Y);
            Refresh();
            OnPropertyChanged("BasketsGathered");

        }
        public void ResetBasketCounter()
        {
            OnPropertyChanged("BasketsGathered");
        }
        public void Refresh()
        {
            foreach (var field in Fields)
            {
                field.IsEmpty = _model.Matrix.GetValue(field.X, field.Y) == 0;
                field.IsMaci = _model.Matrix.GetValue(field.X, field.Y) == 1;
                field.IsHunter = _model.Matrix.GetValue(field.X, field.Y) == 2;
                field.IsBasket = _model.Matrix.GetValue(field.X, field.Y) == 3;
                field.IsTree = _model.Matrix.GetValue(field.X, field.Y) == 4;

                if (field.IsTree)
                    field.IsLocked = true;
            }
            _model.checkGameStatus();
            
        }
        private void Model_GameCreated(object sender, EventArgs e)
        {
            Fields.Clear();
            for (Int32 i = 0; i < _model.FieldSize; i++)
            {
                for (Int32 j = 0; j < _model.FieldSize; j++)
                {
                    GameField temp = new GameField();
                    temp.X = i;
                    temp.Y = j;
                    temp.Number = i * _model.FieldSize + j;
                    temp.StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)));
                    Fields.Add(temp);
                }
            }
            Refresh();
        }

        private void OnGameEnded()
        {
            GameEnded?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameStatusChanged()
        {
            ChangeStatus?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

    }
}
