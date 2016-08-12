namespace ConnectFour {

    public interface IRenderable {
        string Render();
    }
    public interface IGame : IRenderable {
        void Play();
    }

    public class ConnectFourGame: IGame {

        private IBoard board;
        private IPlayer player1;
        private IPlayer player2;

        public ConnectFourGame(IBoard board, IPlayer player1, IPlayer player2) {
            this.board = board;
            this.player1 = player1;
            this.player2 = player2;
        }

        /**
        *   Plays a single move for a given player.
        *   columnIndex and rowIndex will be set from the move that was played.
        *   Continually calls this function until a valid move is played.
        */
        private void playMove(IPlayer player, out int columnIndex, out int rowIndex) {
            System.Console.WriteLine(string.Format("Players {0}'s turn, Please Chose a Number between 0-6", player.Token));
            try {
                columnIndex  = player.Move();
                rowIndex = this.board.PutCounter(columnIndex, player.Token);
            } catch {
                System.Console.WriteLine("The Move Chosen Was Invalid");
                this.playMove(player, out columnIndex, out rowIndex);
            }
        }

        /**
        * Play loop for the game, alternates players go and exists when the game has been won.  
        */
        public void Play() {
            var players = new IPlayer[]{player1, player2};
            var winner = false;
            var winningToken = string.Empty;
            while (!winner) {
                foreach(IPlayer player in players) {
                    System.Console.WriteLine(this.Render());
                    int columnIndex, rowIndex;
                    this.playMove(player, out columnIndex, out rowIndex);
                    winner = this.board.CheckGame(columnIndex, rowIndex);
                    if (winner) {
                        winningToken = player.Token;
                        break;
                    }
                }
            }
            System.Console.WriteLine(this.Render());
            System.Console.WriteLine("Game Over!");
            System.Console.WriteLine(string.Format("The winner was: {0} !", winningToken));
        }

        /**
        *   Wrapper around the board render function.  No additional rendering needed.
        */
        public string Render() {
            return this.board.Render();
        }
    }
}