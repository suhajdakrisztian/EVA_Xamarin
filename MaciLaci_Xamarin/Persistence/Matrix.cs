using System;
using System.Collections.Generic;
using System.Text;

namespace MaciLaci_Xamarin.Persistence
{
    public class Matrix
    {
        #region Fields
        private Int32 _tableSize;

        private Int32[,] _gameMatrix;
        private Tuple<int, int> _MaciCurrent = new Tuple<int, int>(0, 0);

        private Tuple<int, int> _firstHunterCurrent = new Tuple<int, int>(3, 0);
        private string _firstHunterDirection = "right";

        private Tuple<int, int> _secondHunterCurrent = new Tuple<int, int>(9, 4);
        private string _secondHunterDirection = "up";

        public string getHunterDirection(int which)
        {
            if (which < 1 || which > 2)
                throw new ArgumentException("Argument must be at least one and not more than 3");

            if (which == 1)
                return _firstHunterDirection;
            else
                return _secondHunterDirection;

        }

        public bool hunterSeesMaci(Tuple<int, int> a)
        {
            return Math.Abs(a.Item1 - _MaciCurrent.Item1) <= 1 && Math.Abs(a.Item2 - _MaciCurrent.Item2) <= 1;
        }

        public void changeHunterDirection(int which)
        {
            if (which < 1 || which > 2)
                throw new ArgumentException("Argument must be at least one and not more than 3");

            if (which == 1)
            {
                if (_firstHunterDirection == "right")
                    _firstHunterDirection = "left";
                else
                    _firstHunterDirection = "right";

            }
            if (which == 2)
            {
                if (_secondHunterDirection == "up")
                    _secondHunterDirection = "down";
                else
                    _secondHunterDirection = "up";
            }
        }
        public Tuple<int, int> getHunterPosition(int which)
        {
            if (which < 1 || which > 3)
                throw new ArgumentException("Argument must be at least one and not more than 3");

            if (which == 1)
                return _firstHunterCurrent;
            else
                return _secondHunterCurrent;
        }

        public void setHunterPosition(int which, int x, int y)
        {
            if (which < 1 || which > 2)
                throw new ArgumentException("Argument must be 1 or 2");

            if (which == 1)
            {
                _gameMatrix[_firstHunterCurrent.Item1, _firstHunterCurrent.Item2] = 0;
                _firstHunterCurrent = new Tuple<int, int>(x, y);
                _gameMatrix[_firstHunterCurrent.Item1, _firstHunterCurrent.Item2] = 2;
            }
            if (which == 2)
            {
                _gameMatrix[_secondHunterCurrent.Item1, _secondHunterCurrent.Item2] = 0;
                _secondHunterCurrent = new Tuple<int, int>(x, y);
                _gameMatrix[_secondHunterCurrent.Item1, _secondHunterCurrent.Item2] = 2;
            }

        }

        public Tuple<int, int> getCurrentMaciPosition()
        {
            return _MaciCurrent;
        }

        public void setCurrentMaciPosition(int x, int y)
        {
            _gameMatrix[_MaciCurrent.Item1, _MaciCurrent.Item2] = 0;
            _MaciCurrent = new Tuple<int, int>(x, y);
            _gameMatrix[_MaciCurrent.Item1, _MaciCurrent.Item2] = 1;
        }

        public bool isTree(int x, int y)
        {
            return _gameMatrix[x, y] == 4;
        }

        public bool isBasket(int x, int y)
        {
            return _gameMatrix[x, y] == 3;
        }

        #endregion

        #region Properties

        public Int32 TableSize { get { return _tableSize; } }

        #endregion

        #region Constructors

        public Matrix():this(10) { }
        public Matrix(Int32 tableSize)
        {
            if (tableSize < 0)
                throw new ArgumentOutOfRangeException("The table size is less than 0.", "tableSize");
            _tableSize = tableSize;

            _gameMatrix = new Int32[tableSize, tableSize];
            //_fieldLocks = new Boolean[tableSize, tableSize];
        }

        #endregion

        #region Public methods

        public Int32 GetValue(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameMatrix.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _gameMatrix.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");

            return _gameMatrix[x, y];
        }

        public void SetValue(Int32 x, Int32 y, Int32 value)
        {
            if (x < 0 || x >= _gameMatrix.GetLength(0))
                throw new ArgumentOutOfRangeException("x", "The X coordinate is out of range.");
            if (y < 0 || y >= _gameMatrix.GetLength(1))
                throw new ArgumentOutOfRangeException("y", "The Y coordinate is out of range.");
            if (value != 0 && value != 1 && value != 2 && value != 3 && value != 4)
                throw new ArgumentOutOfRangeException("value", "The value is out of range.");

            _gameMatrix[x, y] = value;
        }

        #endregion
    }
}
