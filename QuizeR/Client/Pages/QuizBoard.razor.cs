using Microsoft.AspNetCore.Components;
using QuizeR.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizeR.Client.Pages
{
    public partial class QuizBoard
    {
        [Inject]
        public QuizService QuizService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public int CountDown { get; private set; } = 30;

        public string Question { get; private set; } = string.Empty; 

        public int Score { get; private set; } = 0;

        public bool IsAnswersChoiceVisible { get; private set; } = true;
        public bool AnswerWasCorrect { get; private set; }
        public string Outcome { get; private set; } = string.Empty;

        protected override void OnInitialized()
        {
            QuizService.TimeElapsed += countDownSeconds =>
            {
                CountDown = countDownSeconds;
                StateHasChanged();
            };

            QuizService.QuestionSent += question =>
            {
                Question = question;
                StateHasChanged();
            };

            QuizService.SentRightAnswer += newScore =>
            {
                Score = newScore;
                Outcome = "Nice, your answer is right!";
                AnswerWasCorrect = true;
                StateHasChanged();
            };

            QuizService.SentWrongAnswer += newScore =>
            {
                Score = newScore;
                Outcome = "Oh no, your answer is wrong!";
                AnswerWasCorrect = false;
                StateHasChanged();
            };

            QuizService.RoundEnded += () => NavigationManager.NavigateTo("/ScoreBoard");

            QuizService.GameStopped += () => NavigationManager.NavigateTo("/ScoreBoard");
        }

        public async Task ChooseAnswer(string chosenAnswer)
        {
            IsAnswersChoiceVisible = false;
            await QuizService.ChooseAnswer(chosenAnswer);
        }
    }
}
