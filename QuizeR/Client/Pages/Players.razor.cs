using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class Players
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public QuizService QuizService { get; set; }
        
        public ICollection<string> PlayerNames { get; private set; } = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            QuizService.IncomingPlayers += playerNames =>
            {
                PlayerNames = playerNames;
                StateHasChanged();
            };

            QuizService.GameStarted += () => NavigationManager.NavigateTo("/QuizBoard");

            await QuizService.LoadPlayers();
        }
    }
}
