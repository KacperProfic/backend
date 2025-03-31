using AutoMapper;
using BackendLab01;
using Microsoft.AspNetCore.Mvc;
using Dto;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/quizzes")]
public class QuizController : ControllerBase
{
    private readonly IQuizUserService _service;
    private readonly IMapper _mapper;

    public QuizController(IQuizUserService service,IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
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
    [HttpGet]
    [Route("{quizId}/feedback/{userId}")]
    public ActionResult<FeedbackDto> GetQuizFeedback(int quizId, int userId)
    {
        var quiz = _service.FindQuizById(quizId);
        if (quiz is null) return NotFound();

        var feedback = _service.GetUserAnswersForQuiz(quizId, userId);
        var feedbackDto = new FeedbackDto
        {
            QuizId = quizId,
            UserId = userId,
            TotalQuestions = quiz.Items.Count,
            Answers = _mapper.Map<IEnumerable<FeedbackDto.AnswerFeedbackDto>>(feedback)
        };
        return Ok(feedbackDto);
    }
}