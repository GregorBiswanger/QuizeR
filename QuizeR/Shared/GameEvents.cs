namespace QuizeR.Shared
{
    public static class GameEvents
    {
        public static readonly string GameSessionStarted = "GameSessionStarted";
        public static readonly string PlayerCreated = "PlayerCreated";
        public static readonly string GameStarted = "GameStarted";
        public static readonly string TimeElapsed = "TimeElapsed";
        public static readonly string Question = "Question";
        public static readonly string RightAnswer = "RightAnswer";
        public static readonly string WrongAnswer = "WrongAnswer";
        public static readonly string GameStopped = "GameStopped";
        public static readonly string RoundEnded = "RoundEnded";
        public static readonly string AnswerAndPlayers = "AnswerAndPlayers";
        public static readonly string NextRoundStarted = "NextRoundStarted";
    }
}