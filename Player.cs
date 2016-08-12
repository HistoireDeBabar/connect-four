using System;

namespace ConnectFour {
    public interface IPlayer {
        int Move();
        string Token { get; }
    }

    /**
    * Simple Human Player Class, takes a players move and returns an input
    */
    public class HumanPlayer: IPlayer {
        public string Token { get; }
        public HumanPlayer(string token) {
            this.Token = token;
        }

        /**
        * Simple Move function, takes input from the console and parses the input.
        * Can throw excpetions.
        */
        public int Move() {
            var input = System.Console.ReadLine();
            return int.Parse(input);
        }
    }

    /**
    * Simple AI that choses a random number to play on the board.
    */
    public class RandomAIPlayer : IPlayer
    {
        public string Token { get; }
        private IBoard Board;
        private Random Randomiser;

        public RandomAIPlayer(string token, IBoard board) {
            this.Board = board;
            this.Token = token;
            this.Randomiser = new Random();
        }

        /**
        * Choses the AI's turn.  Will randomly select a postion on the
        * board with the boards width being the maxiumum postion.
        */
        public int Move()
        {
            var width = Board.Width;
            return this.Randomiser.Next(width);
        }
    }
}