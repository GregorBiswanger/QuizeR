using System.Collections.Generic;

namespace QuizeR.Shared
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
