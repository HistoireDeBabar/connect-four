using Xunit;
using ConnectFour;

namespace Tests {
    public class PlayerTests {

        [Fact]
        public void HumanHasToken() {
            var human = new HumanPlayer("*");
            Assert.Equal(human.Token, "*");
        }

        [Fact]
        public void RandomAIPlayer() {
            var raP = new RandomAIPlayer("0", new ConnectFourBoard(4, 4, 4));
            var result = raP.Move();
            Assert.InRange(result, 0, 6);
        }
    }
}