using ApplicationCore.Commons.Repository;
using BackendLab01;

namespace ApplicationCore.Models.QuizAggregate;

public class Quiz : IIdentity<int>
{
    public Quiz()
    {
        Id = 0; 
        Title = string.Empty; 
        Items = new List<QuizItem>(); 
    }
    
    public Quiz(int id, List<QuizItem> items, string title)
    {
        Id = id;
        Title = title;
        Items = items;
    }
    public int Id { get; set; } 
    public string Title { get; set; } 
    public List<QuizItem> Items { get; } 
}