using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFour
{

    public interface IBoard : IRenderable {
        int PutCounter(int index, string counter);
        bool CheckGame(int column, int row);
        int Height { get; }
        int Width { get; }
    }
    public class ConnectFourBoard: IBoard {
        public List<List<string>> Board { get; set; }
        public int Height { get; }
        public int Width { get; }
        private int winningNumber;
        public ConnectFourBoard(int winningNumber = 4, int maxHeight = 6, int maxwidth = 7) {
            this.winningNumber = winningNumber;
            this.Height = maxHeight;
            this.Width = maxwidth;
            this.Board = new List<List<string>>();
            for (var i = 0; i < maxwidth; i++) {
                this.Board.Add(new List<string>());
            }
        }

        /**
        * Given a location on the board, check to see if the
        * game has been won.  Return true is the game is over else false.
        */
        public bool CheckGame(int columm, int row) {
            var lists = new List<String>[]{
                this.BuildColumn(columm),
                this.BuildRow(row),
                this.BuildHorizontalLeftList(columm, row), 
                this.BuildHorizontalRightList(columm, row)
            };
            foreach(var hor in lists) {
                var success = this.CheckList(hor);
                if (success) {
                    return true;
                }
            }
            return false;
        }

        /**
        * Puts a 'counter' into a given column.
        */
        public int PutCounter(int index, string counter) {
            var column = this.Board[index];
            if (column.Count >= this.Height) {
                throw new ArgumentException("Column Full");
            }
            column.Add(counter);
            return column.Count - 1;
        }

        /**
        * Given a list of counters, derive whether they're four in a row.
        */
        private bool CheckList(List<string> list) {
            var inARow = 1;
            string previousValue = string.Empty;
            foreach(var counter in list) {
                if (previousValue == counter & previousValue != string.Empty) {
                    inARow++;
                } else {
                    inARow = 1;
                }
                if (inARow >= this.winningNumber) {
                    return true;
                }
                previousValue = counter;
            }
            return false;
        }

        /**
        * Returns the column of the board.
        */
        public List<string> BuildColumn(int columIndex) {
            return this.Board[columIndex];
        }

        /** 
        * Check a particular column for four in a row.
        **/
        public bool CheckColumn(int columIndex) {
            var column = this.BuildColumn(columIndex);
            return CheckList(column);
        }

        /**
        * Given an index, build a row of the board.
        */
        public List<String> BuildRow(int rowIndex) {
            var row = new List<string>();
            foreach(var column in this.Board) {
                try {
                    row.Add(column[rowIndex]); 
                } catch {
                    row.Add(string.Empty);
                }
            }
            return row;
        }
        /** 
        * Check a particular row for four in a row.
        **/
        public bool CheckRow(int rowIndex) {
            var row = BuildRow(rowIndex);
            return CheckList(row);
        }

        /** 
        * Builds a list based on the horizontal upward direction
        * starting that the bottom left of where the counter coordinates are.
        * Always starts from the bottom row, and continues until there are no more columns.
        **/
        public List<string> BuildHorizontalRightList(int columnIndex, int rowIndex) {
            var row = new List<string>();
            var startColumn = columnIndex - rowIndex;
            var rowContinue = true;
            // the bottom row of the board
            var computedRowIndex = 0;
            while (rowContinue) {
                string nextBlock;
                if (startColumn > this.Board.Count -1) {
                    rowContinue = false;
                    break;
                }
                try {
                    nextBlock = this.Board[startColumn][computedRowIndex];
                } catch {
                    nextBlock = string.Empty;
                }
                row.Add(nextBlock);
                computedRowIndex++;
                startColumn++;
            }
            return row;
        }

        /**
        * Builds a list based on the horizontal upward direction
        * starting that the bottom right of where the counter coordinates are.
        * Always starts on the bottom row, may start off the gird and continues until
        * There are no more columns from right to left.
        **/
        public List<string> BuildHorizontalLeftList(int columnIndex, int rowIndex) {
            var row = new List<string>();
            var computedColumn = columnIndex + rowIndex;
            var rowContinue = true;
            // the bottom row of the board
             var computedRowIndex = 0;
            while (rowContinue) {
                string nextBlock;
                // When you hit the 'left side' of the board stop
                if (computedColumn < 0) {
                    rowContinue = false;
                    break;
                }
                try {
                    nextBlock = this.Board[computedColumn][computedRowIndex];
                } catch {
                    nextBlock = string.Empty;
                }
                row.Add(nextBlock);
                computedRowIndex++;
                computedColumn--;
            }
            return row;
        }

        /**
        * Given a board coordinate, check the horizontal row which
        * the counter is apart from.  The horizontal left is left and up,
        * right and down.
        **/
        public bool CheckLeftHorizontal(int columIndex, int rowIndex) {
            var horizontalLeft = this.BuildHorizontalLeftList(columIndex, rowIndex);
            return CheckList(horizontalLeft);
        }

        /**
        * Given a board coordinate, check the horizontal row which
        * the counter is apart from.  The horizontal right is right and up,
        * left and down.
        **/
        public bool CheckRightHorizontal(int columIndex, int rowIndex) {
            var horizontalright = this.BuildHorizontalRightList(columIndex, rowIndex);
            return CheckList(horizontalright);
        }


        /**
        * Returns the string representation of the board.
        */
        public string Render() {
            var render = string.Empty;
            for (var i = this.Height -1; i >= 0; i --) {
                var rowContent = this.BuildRow(i);
                var renderedContent = rowContent.Select(x => {
                    var y = "  ";
                    if (x == string.Empty) {
                        x = " ";
                    }
                    y += x + "  ";
                    return y;
                });
                render += "||" + String.Join("||", renderedContent) + "|| \n";
            }
            return render;
        }
        
    }

}