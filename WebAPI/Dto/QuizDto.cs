using ApplicationCore.Models.QuizAggregate;

namespace Dto;

public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<QuizItemDto> Items { get; set; }

    public QuizDto(int id, string title, List<QuizItemDto> items)
    {
        Id = id;
        Title = title;
        Items = items;
    }

    public static QuizDto Of(Quiz quiz)
    {
        var items = quiz.Items.Select(QuizItemDto.Of).ToList();
        return new QuizDto(quiz.Id, quiz.Title, items);
    }
}