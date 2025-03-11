using BackendLab01;
using Microsoft.AspNetCore.Mvc;
using Dto;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/quizzes")]
public class QuizController : ControllerBase
{
    private readonly IQuizUserService _service;

    public QuizController(IQuizUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<QuizDto> FindById(int id)
    {
        var quiz = _service.FindQuizById(id);
        if (quiz == null)
        {
            return NotFound();
        }
        return Ok(QuizDto.Of(quiz));
    }

    [HttpGet]
    public IEnumerable<QuizDto> FindAll()
    {
        return _service.FindAllQuizzes().Select(QuizDto.Of);
    }

    [HttpPost]
    [Route("{quizId}/items/{itemId}")]
    public void SaveAnswer([FromBody] QuizItemAnswerDto dto, int quizId, int itemId)
    {
        _service.SaveUserAnswerForQuiz(quizId, dto.UserId, itemId, dto.Answer);
    }

    [HttpGet]
    [Route("{quizId}/users/{userId}/correct-answers")]
    public ActionResult<QuizScoreDto> GetCorrectAnswersCount(int quizId, int userId)
    {
        var count = _service.CountCorrectAnswersForQuizFilledByUser(quizId, userId);
        return Ok(new QuizScoreDto { QuizId = quizId, UserId = userId, CorrectAnswers = count });
    }
}