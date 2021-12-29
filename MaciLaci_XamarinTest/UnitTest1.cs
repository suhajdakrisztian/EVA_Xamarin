using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaciLaci_Xamarin.Model;
using MaciLaci_Xamarin.Persistence;
using System;
using System.IO;

namespace MaciLaci_XamarinTest
{
    [TestClass]
    public class MaciLaciUnitTest
    {
        private GameModel _model;
        private Matrix _matrix;


        [TestInitialize]
        public void Initialize()
        {

            _model = new GameModel();
            _matrix = new Matrix(10);
            _model.SetTable(_matrix);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _matrix.SetValue(i, j, 0);
                }
            }
        }

        [TestMethod]
        public void MaciPosition_SetGet()
        {
            _matrix.setCurrentMaciPosition(0, 0);
            Tuple<int, int> expected = new Tuple<int, int>(0, 0);
            Assert.AreEqual(expected, _matrix.getCurrentMaciPosition());
        }

        [TestMethod]
        public void HunterPosition_SetGet()
        {
            _matrix.setHunterPosition(1, 1, 1);
            Tuple<int, int> expected = new Tuple<int, int>(1, 1);
            Assert.AreEqual(expected, _matrix.getHunterPosition(1));
        }

        [TestMethod]
        public void tableSizeCheck()
        {
            Assert.AreEqual(_matrix.TableSize, 10);
        }

        [TestMethod]
        public void maciCaught()
        {
            _matrix.setCurrentMaciPosition(0, 0);

            _matrix.setHunterPosition(1, 1, 1);
            Tuple<int, int> pos = new Tuple<int, int>(1, 1);
            Assert.AreEqual(true, _matrix.hunterSeesMaci(pos));

            _matrix.setHunterPosition(1, 2, 1);
            Tuple<int, int> pos2 = new Tuple<int, int>(0, 1);
            Assert.AreEqual(true, _matrix.hunterSeesMaci(pos2));

        }
        public void maciNotCaught()
        {
            _matrix.setCurrentMaciPosition(0, 0);

            _matrix.setHunterPosition(1, 3, 3);
            Tuple<int, int> pos = new Tuple<int, int>(3, 3);
            Assert.AreEqual(false, _matrix.hunterSeesMaci(pos));

        }

        [TestMethod]
        public void BasketTreeDetected()
        {

            _matrix.SetValue(3, 3, 3);
            _matrix.SetValue(4, 4, 4);

            Assert.AreEqual(true, _matrix.isBasket(3, 3));
            Assert.AreEqual(true, _matrix.isTree(4, 4));
        }
        [TestMethod]
        public void gameIsWon()
        {
            _matrix.setCurrentMaciPosition(0, 0);
            _matrix.setHunterPosition(1, 3, 3);
            _matrix.setHunterPosition(2, 5, 5);

            _model.checkGameStatus();

            Assert.AreEqual(0, _model.getBasketGathered());
            Assert.AreEqual(true, _model.getGameState() && _model.getMaciWon());
        }

        [TestMethod]
        public void gameIsLost()
        {
            _matrix.setCurrentMaciPosition(0, 0);
            _matrix.setHunterPosition(1, 1, 1);
            _matrix.setHunterPosition(2, 0, 1);

            _model.checkGameStatus();

            Assert.AreEqual(true, _model.getGameState() && _model.getVisibility());

        }

        [TestMethod]
        public void HunterMove()
        {
            _matrix.setHunterPosition(1, 3, 3);

            _model.PlayerMove(0, 0);
            Tuple<int, int> expected = new Tuple<int, int>(3, 4);
            _model.hunterMove(1);
            Assert.AreEqual(expected, _matrix.getHunterPosition(1));

        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileDoesNotExist()
        {
            _model.ReadFile("./nincsilyenfile.txt");
        }

        [TestMethod]
        public void FileExists()
        {
            string x = Directory.GetCurrentDirectory();
            GameModel temp = new GameModel();
            temp.ReadFile("./input.txt");

            Assert.AreEqual(temp.BasketCount, 5);
            Assert.AreEqual(true, true);
        }
    }
}
