using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using QuizeR.Shared;
using System.Collections.Concurrent;
using System.Linq;
using QuizeR.Server.Services;
using System.Timers;

namespace QuizeR.Server.Hubs
{
    public class QuizHub : Hub
    {
        private static ConcurrentDictionary<string, Player> _players = new ConcurrentDictionary<string, Player>();
        private static string _adminConnectionId = string.Empty;
        private QuizDataService quizDataService;
        private static bool _isGameRunning = false;
        private static int _levelIndex = -1; // round of the current game, -1 = no round started yet 
        private static int _seconds = 30;
        private static Quiz _currentQuiz = new Quiz();
        private readonly IHubContext<QuizHub> _hubContext;
        private static int _missingAnswersInRound = -1;

        private static Timer _timer = null;

        public QuizHub(QuizDataService quizDataService, IHubContext<QuizHub> hubContext)
        {
            this.quizDataService = quizDataService;
            _hubContext = hubContext;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("IsSessionOpen", !string.IsNullOrEmpty(_adminConnectionId));

            await base.OnConnectedAsync();
        }

        public async Task StartGameSession()
        {
            _adminConnectionId = Context.ConnectionId;
            await Clients.All.SendAsync(GameEvents.GameSessionStarted);
        }

        public async Task NewPlayer(string playerName)
        {
            if (_players.TryAdd(Context.ConnectionId, new Player { Name = playerName }))
            {
                await Clients.Caller.SendAsync(GameEvents.PlayerCreated);
            }
        }

        public async Task SendPlayers()
        {
            await Clients.All.SendAsync("IncomingPlayers", _players.Values.Select(player => player.Name));
        }

        public async Task StartGame()
        {
            _isGameRunning = true;
            _levelIndex = -1;
            _currentQuiz = quizDataService.LoadQuiz();

            await Clients.All.SendAsync(GameEvents.GameStarted);
            await StartNextLevel();
        }

        public async Task StartNextLevel()
        {
            if(_isGameRunning && _levelIndex < _currentQuiz.Questions.Count - 1)
            {
                _missingAnswersInRound = _players.Count;
                _levelIndex += 1;

                await Clients.All.SendAsync(GameEvents.NextRoundStarted);
                await Clients.All.SendAsync(GameEvents.Question, _currentQuiz.Questions[_levelIndex].Title);
                _seconds = 30;

                if (_timer != null)
                {
                    _timer.Stop();
                }

                _timer = new Timer(1000);
                _timer.AutoReset = true;
                _timer.Elapsed += async (s, e) =>
                {
                    _seconds -= 1;

                    if(_seconds >= 0)
                    {
                        await _hubContext.Clients.All.SendAsync(GameEvents.TimeElapsed, _seconds);
                    }
                    else
                    {
                        await FinishRound();
                    }
                };

                _timer.Start();
            }
        }

        private async Task FinishRound()
        {
            _seconds = 0;
            _timer?.Stop();

            await _hubContext.Clients.All.SendAsync(GameEvents.RoundEnded);
            if (_levelIndex >= _currentQuiz.Questions.Count - 1)
            {
                // no more questions available!
                await StopGame();
            }
        }

        private async Task StopGame()
        {
            _isGameRunning = false;
            await _hubContext.Clients.All.SendAsync(GameEvents.GameStopped);
        }

        public async Task PlayerAnswer(string answer)
        {
            if(_currentQuiz.Questions[_levelIndex].RightAnswer == answer)
            {
                _players[Context.ConnectionId].Score += _seconds;
                await Clients.Caller.SendAsync(GameEvents.RightAnswer, _players[Context.ConnectionId].Score);
            }
            else
            {
                await Clients.Caller.SendAsync(GameEvents.WrongAnswer, _players[Context.ConnectionId].Score);
            }

            _missingAnswersInRound--;
            if (_missingAnswersInRound <= 0)
            {
                await FinishRound();
            }
        }

        public async Task GetAnswerAndPlayers()
        {
            await Clients.All.SendAsync(GameEvents.AnswerAndPlayers, new AnswerAndPlayers
            {
                RightAnswer = _currentQuiz.Questions[_levelIndex].RightAnswer,
                Players = _players.Values.OrderByDescending(player => player.Score).ToList()
            });
        }
    }
}
