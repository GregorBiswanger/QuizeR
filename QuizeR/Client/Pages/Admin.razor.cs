using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class Admin
    {
        [Inject]
        public QuizService QuizService { get; set; }

        public bool GameIsRunning { get; set; }

        public bool NextRoundPending { get; private set; }

        protected override void OnInitialized()
        {
            //QuizService.GameStarted += () =>
            //{
            //    GameIsRunning = true;
            //    StateHasChanged();
            //};

            //QuizService.GameStopped += () =>
            //{
            //    GameIsRunning = false;
            //    StateHasChanged();
            //};

            //QuizService.RoundEnded += () =>
            //{
            //    NextRoundPending = true;
            //    StateHasChanged();
            //};
        }

        public async Task StartGame()
        {
            await QuizService.StartGame();
        }

        public async Task StartNextRound()
        {
            //NextRoundPending = false;
            await QuizService.StartNextRound();
        }
    }
}
