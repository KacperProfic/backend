using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages.Quiz;

public class IndexModel : PageModel
{
    private readonly IQuizAdminService _admin;

    public IndexModel(IQuizAdminService admin)
    {
        _admin = admin;
    }

    [BindProperty] 
    public List<ApplicationCore.Models.QuizAggregate.Quiz> Quizzes { get; set; }
    
    public void OnGet()
    {
        Quizzes = _admin.FindAllQuizzes();
    }
}