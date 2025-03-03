using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackendLab01.Pages.Quiz;

public class Summary : PageModel
{   
    private readonly IQuizUserService _userService;
    
    public int CorrectAnswerCount { get; set; }
    public Summary(IQuizUserService userService)
    {
        _userService = userService;
    }

    public void OnGet(int quizId, int userId)
    {
        CorrectAnswerCount = _userService.CountCorrectAnswersForQuizFilledByUser(quizId, userId);
    }
}