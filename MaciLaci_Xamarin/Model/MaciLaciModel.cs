using System;
using System.Diagnostics;
using System.IO;
using MaciLaci_Xamarin.Persistence;


namespace MaciLaci_Xamarin.Model
{
    public class GameModel
    {
        private Matrix _matrix;
        private int _BasketCount;
        private int _basketGathered = 0;
        
        private bool _gameOver;
        private bool _hunterSees;
        private bool _maciWon;

        private bool _GameRunning;

        public int FieldSize 
        {
            get => _matrix.TableSize;
        }

        //testing only
        public bool getGameState()
        {
            return _gameOver;
        }
        public bool getVisibility()
        {
            return _hunterSees;
        }
        public bool getMaciWon()
        {
            return _maciWon;
        }

        public event EventHandler<EventArgs> GameOverMaciWon;
        public event EventHandler<EventArgs> GameOverMaciLost;
        public event EventHandler<EventArgs> GameCreated;

        public void NewGame(int fieldSize = 10)
        {
            string pathToFile = "./input.txt";

            if (fieldSize == 10)
            {
                pathToFile = "./input.txt";
            }
            else if (fieldSize == 11)
            {
                pathToFile = "./input1.txt";
            }
            else if (fieldSize == 12)
            {
                pathToFile = "./input2.txt";
            }
            ReadFile(pathToFile);
            OnGameCreated();
            _GameRunning = true;

        }

        public void OnGameCreated()
        {
            GameCreated?.Invoke(this, EventArgs.Empty);
        }

        public void SetTable(Matrix table)
        {
            _matrix = table;
        }
        public int BasketCount
        {
            get => _BasketCount;
            set => _BasketCount = value;
        }
        public int getBasketGathered()
        {
            return _basketGathered;
        }
        public void SetBasketGathered()
        {
            _basketGathered = 0;
        }
        public void checkGameStatus()
        {
            if (_BasketCount == 0 && !_matrix.hunterSeesMaci(_matrix.getHunterPosition(1)) && !_matrix.hunterSeesMaci(_matrix.getHunterPosition(2)))
            {
                _gameOver = true;
                _maciWon = true;
            }
            else if (_matrix.hunterSeesMaci(_matrix.getHunterPosition(1)) || _matrix.hunterSeesMaci(_matrix.getHunterPosition(2)))
            {
                _gameOver = true;
                _hunterSees = true;
                _maciWon = false;
            }
            else
            {
                _gameOver = false;
                _hunterSees = false;
                _maciWon = false;
            }

            // ha valami tortenik, itt kezeli
            if (_gameOver && _maciWon && _GameRunning)
            {
                _GameRunning = false;
                GameOverMaciWon?.Invoke(this, null);
            }
            else if (_gameOver && _hunterSees && _GameRunning)
            {
                _GameRunning = false;
                GameOverMaciLost?.Invoke(this, null);
            }
        }
        public void hunterMove(int which)
        {
            Tuple<int, int> current;
            if (which == 1)
            {
                current = _matrix.getHunterPosition(1);
            }
            else
            {
                current = _matrix.getHunterPosition(2);
            }

            switch (_matrix.getHunterDirection(which))
            {
                case "up":
                    _matrix.setHunterPosition(2, Math.Max(0, current.Item1 - 1), current.Item2);
                    if (Math.Max(0, current.Item1 - 1) == 0)
                        _matrix.changeHunterDirection(2);
                    break;
                case "down":
                    _matrix.setHunterPosition(2, Math.Min(current.Item1 + 1, _matrix.TableSize - 1), current.Item2);
                    if (Math.Min(current.Item1 + 1, _matrix.TableSize - 1) == _matrix.TableSize - 1)
                        _matrix.changeHunterDirection(2);
                    break;
                case "left":
                    _matrix.setHunterPosition(1, current.Item1, Math.Max(current.Item2 - 1, 0));
                    if (Math.Max(current.Item2 - 1, 0) == 0)
                        _matrix.changeHunterDirection(1);
                    break;
                case "right":
                    _matrix.setHunterPosition(1, current.Item1, Math.Min(current.Item2 + 1, _matrix.TableSize - 1));
                    if (Math.Min(current.Item2 + 1, _matrix.TableSize - 1) == _matrix.TableSize - 1)
                        _matrix.changeHunterDirection(1);
                    break;
                default:
                    break;
            }
            _hunterSees = _matrix.hunterSeesMaci(_matrix.getHunterPosition(1)) || _matrix.hunterSeesMaci(_matrix.getHunterPosition(2));
            //checkGameStatus();
        }
        public void PlayerMove(int x, int y)
        {
            Tuple<int, int> res = _matrix.getCurrentMaciPosition();

            if (Math.Abs(res.Item1 - x) == 1 && res.Item2 == y && _matrix.GetValue(x, y) != 4 && _matrix.GetValue(x, y) != 2)
            {
                if (_matrix.GetValue(x, y) == 3)
                {
                    _BasketCount--;
                    _basketGathered++;
                    //_matrix.SetValue(x, y, 1);
                }
                _matrix.setCurrentMaciPosition(x, y);
            }
            else if (Math.Abs(res.Item2 - y) == 1 && res.Item1 == x && _matrix.GetValue(x, y) != 4 && _matrix.GetValue(x, y) != 2)
            {
                if (_matrix.GetValue(x, y) == 3)
                {
                    _BasketCount--;
                    _basketGathered++;
                    //_matrix.SetValue(x, y, 1);
                }
                _matrix.setCurrentMaciPosition(x, y);
            }

            _maciWon = _BasketCount == 0 && !_matrix.hunterSeesMaci(_matrix.getHunterPosition(1)) && !_matrix.hunterSeesMaci(_matrix.getHunterPosition(2));
        }
        public GameModel()
        {
            _matrix = new Matrix();
        }

        public Matrix Matrix
        {
            get { return _matrix; }
        }

        public void ReadFile(string path)
        {

            if (!File.Exists(path)) throw new FileNotFoundException("No Such file");


            StreamReader streamReader = new StreamReader(path);

            string line = streamReader.ReadLine();
            string[] numbers = line.Split(' ');


            int tableSize = int.Parse(numbers[0]);
            Matrix table = new Matrix(tableSize);

            _BasketCount = Int32.Parse(numbers[1]);

            for (Int32 i = 0; i < tableSize; i++)
            {
                line = streamReader.ReadLine();
                numbers = line.Split(' ');

                for (Int32 j = 0; j < tableSize; j++)
                {
                    table.SetValue(i, j, Int32.Parse(numbers[j]));
                }
            }
            _matrix = table;

        }
    }
}
