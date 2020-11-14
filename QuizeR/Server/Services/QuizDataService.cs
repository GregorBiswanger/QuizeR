using QuizeR.Shared;
using System.Collections.Generic;

namespace QuizeR.Server.Services
{
    public class QuizDataService
    {
        public Quiz LoadQuiz()
        {
            var quiz = new Quiz
            {
                Id = 1,
                Title = "SignalR Quiz",
                Questions = new List<Question>
                {
                    new Question { Title = "Which of the following operator returns the address of an variable in C#? - A - sizeof # B - typeof # C - & # D - *", RightAnswer = "C" },
                    new Question { Title = "Which of the following method helps in returning more than one value? # A - Value parameters # B - Reference parameters # C - Output parameters # D - None of the above.", RightAnswer = "C" },
                    new Question { Title = "Which of the following property of Array class in C# gets a 64-bit integer, the total number of elements in all the dimensions of the Array? - A - Rank # B - LongLength # C - Length # D - None of the above.", RightAnswer = "B" },
                    new Question { Title = "Which of the following preprocessor directive defines a sequence of characters as symbol in C#? - A - define # B - undef # C - region # D - endregion", RightAnswer = "A" }
                }
            };

            return quiz;
        }
    }
}
