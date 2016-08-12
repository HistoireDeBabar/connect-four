using System;

namespace ConnectFour
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome To Connect Four!");
            var board = new ConnectFourBoard();
            var human = new HumanPlayer("*");
            var computer = new RandomAIPlayer("0", board);
            var game = new ConnectFourGame(board, human, computer);
            game.Play();
        }
    }
}
