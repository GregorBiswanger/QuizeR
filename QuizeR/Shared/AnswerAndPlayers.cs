using System.Collections.Generic;

namespace QuizeR.Shared
{
    public class AnswerAndPlayers
    {
        public string RightAnswer { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
