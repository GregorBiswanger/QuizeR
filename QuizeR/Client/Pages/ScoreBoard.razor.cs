using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using QuizeR.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class ScoreBoard
    {
        [Inject]
        public QuizService QuizService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool IsGameOver { get; set; } = false;

        public string Answer { get; private set; } = string.Empty;

        public ICollection<Player> Players { get; private set; } = new List<Player>();

        protected override async Task OnInitializedAsync()
        {
            QuizService.GotAnswerAndPlayers += (answerAndPlayer) =>
            {
                Answer = answerAndPlayer.RightAnswer;
                Players = answerAndPlayer.Players;
                StateHasChanged();
            };

            QuizService.NextRoundStarted += () => NavigationManager.NavigateTo("/QuizBoard");
            QuizService.GameStopped += () =>
            {
                IsGameOver = true;
                StateHasChanged();
            };

            await QuizService.GetAnswerAndPlayers();
        }
    }
}
