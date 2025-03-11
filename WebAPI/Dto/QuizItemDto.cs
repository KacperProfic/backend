using ApplicationCore.Models.QuizAggregate;

namespace Dto;

public class QuizItemDto
{
    public int Id { get; set; }
    public string Question { get; set; }
    public List<string> Options { get; set; }

    public QuizItemDto(int id, string question, List<string> options)
    {
        Id = id;
        Question = question;
        Options = options;
    }

    public static QuizItemDto Of(QuizItem quiz)
    {
        var options = new List<string>(quiz.IncorrectAnswers) { quiz.CorrectAnswer };
        options = options.OrderBy(x => Guid.NewGuid()).ToList(); // Randomize order
        return new QuizItemDto(quiz.Id, quiz.Question, options);
    }
}