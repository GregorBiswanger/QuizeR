using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizeR.Shared;

namespace QuizeR.Client.Services
{
    public class QuizService
    {
        private HubConnection _hubConnection;
        private readonly string _hubUri;

        public bool IsAdmin { get; private set; }

        public event Action GameSessionStarted;
        public event Action<bool> IsSessionOpen;
        public event Action PlayerCreated;
        public event Action<ICollection<string>> IncomingPlayers;
        public event Action GameStarted;
        public event Action<int> TimeElapsed;
        public event Action<string> QuestionSent;
        public event Action GameStopped;
        public event Action RoundEnded;
        public event Action<int> SentRightAnswer;
        public event Action<int> SentWrongAnswer;
        public event Action<AnswerAndPlayers> GotAnswerAndPlayers;
        public event Action NextRoundStarted;

        public QuizService(string hubUri)
        {
            _hubUri = hubUri;
        }
        
        public async Task Connect()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(_hubUri).Build();
            _hubConnection.On(GameEvents.GameSessionStarted, () => GameSessionStarted?.Invoke());
            _hubConnection.On<bool>(nameof(IsSessionOpen), (isOpen) => IsSessionOpen?.Invoke(isOpen));
            _hubConnection.On(GameEvents.PlayerCreated, () => PlayerCreated?.Invoke());
            _hubConnection.On<ICollection<string>>(nameof(IncomingPlayers), (players) => IncomingPlayers?.Invoke(players));
            _hubConnection.On(GameEvents.GameStarted, () => GameStarted?.Invoke());
            _hubConnection.On<int>(GameEvents.TimeElapsed, countDownSeconds => TimeElapsed?.Invoke(countDownSeconds));
            _hubConnection.On(GameEvents.GameStopped, () => GameStopped?.Invoke());
            _hubConnection.On<int>(GameEvents.RightAnswer, newScore => SentRightAnswer?.Invoke(newScore));
            _hubConnection.On<int>(GameEvents.WrongAnswer, newScore => SentWrongAnswer?.Invoke(newScore));
            _hubConnection.On(GameEvents.RoundEnded, () => RoundEnded?.Invoke());
            _hubConnection.On<string>(GameEvents.Question, question => QuestionSent?.Invoke(question));
            _hubConnection.On<AnswerAndPlayers>(GameEvents.AnswerAndPlayers, answerAndPlayers => GotAnswerAndPlayers?.Invoke(answerAndPlayers));
            _hubConnection.On(GameEvents.NextRoundStarted, () => NextRoundStarted?.Invoke());

            await _hubConnection.StartAsync();
        }

        public void StartGameSession()
        {
            _hubConnection.SendAsync("StartGameSession");
            IsAdmin = true;
        }

        public async Task NewPlayer(string playerName)
        {
            await _hubConnection.SendAsync("NewPlayer", playerName);
        }

        public async Task LoadPlayers()
        {
            await _hubConnection.SendAsync("SendPlayers");
        }

        public async Task StartGame()
        {
            await _hubConnection.SendAsync("StartGame");
        }

        public async Task ChooseAnswer(string chosenAnswer)
        {
            await _hubConnection.SendAsync("PlayerAnswer", chosenAnswer);
        }

        public async Task GetAnswerAndPlayers()
        {
            await _hubConnection.SendAsync("GetAnswerAndPlayers");
        }

        public async Task StartNextRound()
        {
            await _hubConnection.SendAsync("StartNextLevel");
        }
    }
}
