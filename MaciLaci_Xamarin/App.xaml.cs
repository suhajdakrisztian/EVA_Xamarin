using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MaciLaci_Xamarin.Model;
using MaciLaci_Xamarin.ViewModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MaciLaci_Xamarin
{
    public partial class App : Application
    {
        private MaciLaciViewModel _viewModel;
        private GameModel _model;
        private bool TimerEnabled = true;

        private int _currentTime = 0;

        public int CurrentTime
        {
            set => _currentTime = value;
            get => _currentTime;
        }



        public App()
        {
            InitializeComponent();

            _model = new GameModel();
            _viewModel = new MaciLaciViewModel(_model);

            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ChangeStatus += new EventHandler(ViewModel_GameStatusChanged);

            _model.GameOverMaciWon += new EventHandler<EventArgs>(Model_GameOverMaciWonAsync);
            _model.GameOverMaciLost += new EventHandler<EventArgs>(Model_GameOverMaciLostAsync);

            MainPage = new MainPage
            {
                BindingContext = _viewModel
            };

        }

        public void ChangeTimer()
        {
            if (TimerEnabled)
                TimerEnabled = false;
            else
                TimerEnabled = true;
        }

        protected override void OnStart()
        {
            _model.NewGame();
            Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });
        }

        private void TimerTick()
        {
            _viewModel.TimeElapsed++;

            _model.hunterMove(1);
            _model.hunterMove(2);
            _viewModel.Refresh();
        }

        protected override void OnSleep()
        {
            TimerEnabled = true;
        }

        protected override void OnResume()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });
        }
        private async void Model_GameOverMaciWonAsync(Object sender, EventArgs e)
        {
            TimerEnabled = false;

            foreach (GameField field in _viewModel.Fields)
                field.IsLocked = true;

            var result = await MainPage.DisplayAlert("Játék vége", "Nyertél", "New Game", "Kilépés a játékból");

            if (!result)
                Environment.Exit(0);

            _model.NewGame(int.Parse(_viewModel.TableSize));
            _model.SetBasketGathered();

            TimerEnabled = true;
            _viewModel.TimeElapsed = 0;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });

            _viewModel.ResetBasketCounter();
        }
        private async void Model_GameOverMaciLostAsync(Object sender, EventArgs e)
        {
            ChangeTimer();

            foreach (GameField field in _viewModel.Fields)
                field.IsLocked = true;

            var result = await MainPage.DisplayAlert("Játék vége ", "Vesztettél","New Game", "Kilépés a játékból");
            
            if (!result)
                Environment.Exit(0);

            _model.NewGame(int.Parse(_viewModel.TableSize));
            _model.SetBasketGathered();

            TimerEnabled = true;
            _viewModel.TimeElapsed = 0;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });

            _viewModel.ResetBasketCounter();
        }

        private void ViewModel_GameStatusChanged(object sender, EventArgs e)
        {
            if (TimerEnabled)
            {
                TimerEnabled = false;

                foreach (var field in _viewModel.Fields)
                {
                    field.IsLocked = true;
                }
            }
            else
            {
                TimerEnabled = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });
                foreach (var field in _viewModel.Fields)
                {
                    field.IsLocked = false;
                }
            }

        }
        private async void ViewModel_NewGame(object sender, EventArgs e)
        {
            TimerEnabled = false;

            foreach (var field in _viewModel.Fields)
            {
                field.IsLocked = true;
            }

            string result = await MainPage.DisplayActionSheet("Mekkora legyen a pálya?", "Vissza", null, "10", "11", "12");


            int temp = int.Parse(result);

            _model.NewGame(temp);
            _model.SetBasketGathered();

            TimerEnabled = true;
            _viewModel.TimeElapsed = 0;
            Device.StartTimer(TimeSpan.FromSeconds(1), () => { TimerTick(); return TimerEnabled; });

            _viewModel.ResetBasketCounter();
            _viewModel.TableSize = result;

        }
    }
}


