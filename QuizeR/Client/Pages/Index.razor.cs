using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public QuizService QuizService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            QuizService.IsSessionOpen += (isOpen) =>
            {
                if(isOpen)
                {
                    NavigationManager.NavigateTo("/NewPlayer");
                }
            };

            QuizService.GameSessionStarted += () =>
            {
                if (QuizService.IsAdmin)
                {
                    NavigationManager.NavigateTo("/Players");
                }
                else
                {
                    NavigationManager.NavigateTo("/NewPlayer");
                }
            };

            await QuizService.Connect();
        }

        public void StartGameSession()
        {
            QuizService.StartGameSession();
        }
    }
}
