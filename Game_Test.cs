using Xunit;
using ConnectFour;
using System;

namespace Tests {
    public class GameTests {

        public class MockPlayerThrowsErrorMove: IPlayer {
            public int MoveCount {get; set;}
            public string Token {get;}
            public MockPlayerThrowsErrorMove() {
                Token = "*";
                MoveCount = 0;
            }

            public int Move() {
                if (this.MoveCount >= 4) {
                    return 0;
                }
                this.MoveCount++;
                throw new System.ArgumentOutOfRangeException("Invalid");
            }
        }
        [Fact]
        public void CallsPlayerMoveAgainWhenInitialMoveThrowsError() {
            var board = new ConnectFourBoard();
            var mockPlayer = new MockPlayerThrowsErrorMove();
            var mockPlayer2 = new MockPlayerThrowsErrorMove();
            var game = new ConnectFourGame(board, mockPlayer, mockPlayer2);
            game.Play();
            Assert.Equal(mockPlayer.MoveCount, 4);
            Assert.Equal(mockPlayer2.MoveCount, 4);
        }


        public class MockPlayerReturnsZero: IPlayer {
            public int MoveCount {get; set;}
            public string Token {get;}
            public MockPlayerReturnsZero(string token = "*") {
                MoveCount = 0;
                this.Token = token;
            }

            public int Move() {
                this.MoveCount++;
                return 0;
            }
        }


        public class MockBoardThrowsErrorOnPut: IBoard {
            public int called;

            public int Width {get;}
            public int Height {get;}

            public string Render () { return "Test Render"; }

            public MockBoardThrowsErrorOnPut() {
                called = 0;
            }

            public bool CheckGame(int column, int row)
            {
                return true;
            }

            public int PutCounter(int index, string counter)
            {
                this.called++;
                if (this.called >= 2) {
                    return 0;
                }
                throw new ArgumentException("No More Room");
            }
        }
        [Fact]
        public void CallsPlayerWhenPositionOnBoardIsOutOfBounds() {
            var board = new MockBoardThrowsErrorOnPut();
            var mockPlayer = new MockPlayerReturnsZero();
            var player2 = new MockPlayerReturnsZero();
            var game = new ConnectFourGame(board, mockPlayer, player2);
            game.Play();
            Assert.Equal(board.called, 2);
        }

        public class MockBoardReturnsCheckTrue: IBoard {
            public string WinnersToken;
            public int Width {get;}
            public int Height {get;}
            public string Render () { return "Test Render"; }
            public int Called;
            public MockBoardReturnsCheckTrue() {
                this.WinnersToken = string.Empty;
                this.Called = 0;
            }

            public bool CheckGame(int column, int row)
            {
                return true;
            }

            public int PutCounter(int index, string counter)
            {
                this.Called++;
                this.WinnersToken = counter;
                return 0;
            }
        }
          
        [Fact]
        public void ReturnsFromGameWhenAPlayerWins() {
            var board = new MockBoardReturnsCheckTrue();
            var mockWinner = new MockPlayerReturnsZero("!");
            var mockLoser = new MockPlayerReturnsZero("0");
            var game = new ConnectFourGame(board, mockWinner, mockLoser);
            game.Play();
            Assert.Equal(board.Called, 1);
            Assert.Equal(board.WinnersToken, "!");
        }
    }
}