using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class NewPlayer
    {
        public string PlayerName { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public QuizService QuizService { get; set; }

        protected override void OnInitialized()
        {
            QuizService.PlayerCreated += () => NavigationManager.NavigateTo("/Players");
        }

        public async Task CreateNewPlayer()
        {
            await QuizService.NewPlayer(PlayerName);
        }
    }
}
