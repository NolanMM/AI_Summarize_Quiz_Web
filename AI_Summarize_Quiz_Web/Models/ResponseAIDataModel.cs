namespace AI_Summarize_Quiz_Web.Models
{
    public class ResponseAIDataModel
    {
        public string? Message { get; set; }
        public string? Results { get; set; }
    }
    public class Results
    {
        public List<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public List<TrueFalseQuestion> TrueFalseQuestions { get; set; }
        public List<FillInTheBlankQuestion> FillInTheBlankQuestions { get; set; }
        public List<WordProblemQuestion> WordProblemQuestions { get; set; }
    }

    public class MultipleChoiceQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class TrueFalseQuestion
    {
        public string Statement { get; set; }
        public bool Correct { get; set; }
    }

    public class FillInTheBlankQuestion
    {
        public string Statement { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class WordProblemQuestion
    {
        public string Problem { get; set; }
        public string Answer { get; set; }
    }

}

