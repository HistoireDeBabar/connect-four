using Xunit;
using ConnectFour;
using System.Collections.Generic;

namespace Tests {
    public class BoardTests {

        [Fact]
        public void PutNegativeShouldThrowException() {
            var board = new ConnectFourBoard();
            var ex = Assert.ThrowsAny<System.Exception>(() => board.PutCounter(-1, "*"));
            Assert.Contains("Index was out of range", ex.Message);
        }

        [Fact]
        public void PutOutOfIndexColumnShouldThrowException() {
            var board = new ConnectFourBoard();
            var ex = Assert.ThrowsAny<System.Exception>(() => board.PutCounter(7, "*"));
            Assert.Contains("Index was out of range", ex.Message);
        }

        [Fact]
        public void PutCounterInFullColumnShouldThrowException() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            var ex = Assert.ThrowsAny<System.Exception>(() => board.PutCounter(0, "*"));
            Assert.Contains("Column Full", ex.Message);
        }


        [Fact]
        public void CheckEmptyColumn() {
            var board = new ConnectFourBoard();
            Assert.Equal(false, board.CheckColumn(0));
        }

        [Fact]
        public void CheckEmptyColumnWithNoFourInAColumn() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            board.PutCounter(0, "*");
            board.PutCounter(0, "0");
            Assert.Equal(false, board.CheckColumn(0));
        }
        
        [Fact]
        public void CheckEmptyColumnWithFourInAColumn() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(0, "*");
            board.PutCounter(0, "*");
            board.PutCounter(0, "*");
            Assert.Equal(true, board.CheckColumn(0));
        }

        [Fact]
        public void CheckEmptyRow() {
            var board = new ConnectFourBoard();
            Assert.Equal(false, board.CheckRow(0));
        }

        [Fact]
        public void CheckEmptyRowWithNoFourInARow() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(1, "0");
            board.PutCounter(2, "*");
            board.PutCounter(3, "0");
            board.PutCounter(4, "*");
            board.PutCounter(5, "0");
            board.PutCounter(6, "*");
            Assert.Equal(false, board.CheckRow(0));
        }

        [Fact]
        public void CheckRowWithFourInARow() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(1, "0");
            board.PutCounter(2, "0");
            board.PutCounter(3, "0");
            board.PutCounter(4, "0");
            board.PutCounter(5, "*");
            board.PutCounter(6, "*");
            Assert.Equal(true, board.CheckRow(0));
        }

        [Fact]
        public void CheckRowWithMissingConterFourInARow() {
            var board = new ConnectFourBoard();
            board.PutCounter(0, "*");
            board.PutCounter(1, "0");
            board.PutCounter(2, "0");
            // missing row 3
            board.PutCounter(4, "0");
            board.PutCounter(5, "0");
            board.PutCounter(6, "*");
            Assert.Equal(false, board.CheckRow(0));
        }


        [Fact]
        public void CheckPeakRow() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0", "0" },
                new List<string>() { "0", "0", "0" },
                new List<string>() { "0", "0", "0", "0" },
                new List<string>() { "0", "0", "0" },
                new List<string>() { "0", "0" },
            };
            Assert.Equal(false, board.CheckRow(3));
        }

        [Fact]
        public void BuildHoriztonalFromEmpty() {
            var board = new ConnectFourBoard();
            // builds a list of empty string
            Assert.Equal(7, board.BuildHorizontalRightList(0, 0).Count);
        }

        [Fact]
        public void BuildHoriztonalRightFromSmallList() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "0", "0" },
                new List<string>() { "0", "0", "0" },
                new List<string>() { "0", "0", "0", "0" },
            };
            Assert.Equal(4, board.BuildHorizontalRightList(0, 0).Count);
            Assert.Equal(3, board.BuildHorizontalRightList(1, 0).Count);
            Assert.Equal(2, board.BuildHorizontalRightList(2, 0).Count);
            Assert.Equal(1, board.BuildHorizontalRightList(3, 0).Count);
        }

          [Fact]
        public void BuildHoriztonalRightFromOffBase() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "0", "0" },
                new List<string>() { "0", "0", "0" },
                new List<string>() { "0", "0", "0", "0" },
            };
            Assert.Equal(3, board.BuildHorizontalRightList(2, 1).Count);
        }

        [Fact]
        public void BuildHoriztonalRightFromGappyList() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "*" },
                new List<string>() { "0" },
                new List<string>() { "0", "0", "*" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "0", "*" },
            };
            var horizontalList = board.BuildHorizontalRightList(3,3);
            Assert.Equal(6, horizontalList.Count);
            // The Gap should be an empty string
            Assert.Equal(string.Empty, horizontalList[1]);
            // All other values should be a star
            var stars = new int[]{0, 2, 3, 4, 5};
            for(var i = 0; i < stars.Length; i++) {
                Assert.Equal("*", horizontalList[stars[i]]);
            }
        }

        [Fact]
        public void BuildHoriztonalLeftFromSmallList() {
            var board = new ConnectFourBoard(4, 4);
            board.Board = new List<List<string>>() {
                new List<string>() { "0", "0", "0", "0" },
                new List<string>() { "0", "0", "0" },
                new List<string>() { "0", "0" },
                new List<string>() { "0" },
            };
            Assert.Equal(4, board.BuildHorizontalLeftList(0, 3).Count);
            Assert.Equal(3, board.BuildHorizontalLeftList(1, 1).Count);
            Assert.Equal(2, board.BuildHorizontalLeftList(1, 0).Count);
            Assert.Equal(1, board.BuildHorizontalLeftList(0, 0).Count);
        }


        [Fact]
        public void BuildOutlierHoriztonalLeftFromSmallList() {
            var board = new ConnectFourBoard(4, 4);
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "0" },
                new List<string>() { "0" },
                new List<string>() { "0", "0", "0" },
            };
            var horizontalList = board.BuildHorizontalLeftList(3, 2);
            Assert.Equal(6, horizontalList.Count);
            Assert.Equal("0", horizontalList[2]);
            var empties = new int[]{0, 1, 3, 4, 5};
            for(var i = 0; i < empties.Length; i++) {
                Assert.Equal(string.Empty, horizontalList[empties[i]]);
            }
        }

        [Fact]
        public void BuildHoriztonalLeftFromGappyList() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0", "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0" },
            };
            var horizontalList = board.BuildHorizontalLeftList(3,2);
            Assert.Equal(6, horizontalList.Count);
            // The Gap should be an empty string
            Assert.Equal(string.Empty, horizontalList[1]);
            // All other values should be a star
            var stars = new int[]{0, 2, 3, 4, 5};
            for(var i = 0; i < stars.Length; i++) {
                Assert.Equal("*", horizontalList[stars[i]]);
            }
        }

        [Fact]
        public void CheckHoriztonalLeftFromGappyListSuccess() {
            // Success Path
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0", "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0" },
            };
            Assert.Equal(true, board.CheckLeftHorizontal(3,2));
        }

        [Fact]
        public void CheckHoriztonalLeftFromGappyListFalse() {
            // False Path
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0", "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0" },
            };
            Assert.Equal(false, board.CheckLeftHorizontal(3,2));
        }

        [Fact]
        public void CheckHoriztonalRightFromGappyListSuccess() {
            // Success Path
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "0", "*" },
            };
            Assert.Equal(true, board.CheckRightHorizontal(3,2));
        }

        [Fact]
        public void CheckHoriztonalRightFromGappyListFalse() {
            // False Path
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0", "0", "0", "0" },
                new List<string>() { "0", "0", "0", "0", "*" },
                new List<string>() { "0", "0", "0", "0", "0", "*" },
            };
            Assert.Equal(false, board.CheckRightHorizontal(3,2));
        }

        [Fact]
        public void CheckGameColumn() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "" },
                new List<string>() { "0", "0", "*", "0" },
                new List<string>() { "0", "0", "0", "0" },
                new List<string>() { "*", "*", "0", "0", "*" },
                new List<string>() { "0", "0", "*", "0", "*", "*" },
            };
            Assert.Equal(true, board.CheckGame(4,3));
        }

        [Fact]
        public void CheckGameRow() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "" },
                new List<string>() { "0", "*", "0", "0" },
                new List<string>() { "0", "*", },
                new List<string>() { "*", "*", },
                new List<string>() { "0", "*" },
            };
            Assert.Equal(true, board.CheckGame(6,1));
        }

        [Fact]
        public void CheckGameHorizontalRight() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0" },
                new List<string>() { "0", "*", },
                new List<string>() { "0", "*", "*", "0" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "*", "*", },
                new List<string>() { "0", "*" },
            };
            Assert.Equal(true, board.CheckGame(4,3));
        }

        [Fact]
        public void CheckGameHorizontalLeft() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0", "*", "0" },
                new List<string>() { "0", "*", "0" },
                new List<string>() { "*", "0", "0", "0" },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "*", "*", },
                new List<string>() { "0", "*" },
            };
            Assert.Equal(true, board.CheckGame(1,3));
        }

        [Fact]
        public void CheckGameFailure() {
            var board = new ConnectFourBoard();
            board.Board = new List<List<string>>() {
                new List<string>() { "0" },
                new List<string>() { "*", "0", "*", "0" },
                new List<string>() { "0", "*", "0" },
                new List<string>() { "*", "*", },
                new List<string>() { "0", "0", "0", "*" },
                new List<string>() { "*", "*", },
                new List<string>() { "0", "*" },
            };
            Assert.Equal(false, board.CheckGame(1,3));
        }
    }

}