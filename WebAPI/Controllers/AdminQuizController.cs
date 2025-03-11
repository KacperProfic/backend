using ApplicationCore.Commons.Repository;
using BackendLab01;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Models.QuizAggregate;
using Dto;

namespace BackendLab01.Controllers;

[ApiController]
[Route("api/v1/admin/quizzes")]
public class AdminQuizController : ControllerBase
{
    private readonly IQuizUserService _service;
    private readonly IGenericRepository<Quiz, int> _quizRepository;
    private readonly IGenericRepository<QuizItem, int> _itemRepository;

    public AdminQuizController(IQuizUserService service, IGenericRepository<Quiz, int> quizRepository, IGenericRepository<QuizItem, int> itemRepository)
    {
        _service = service;
        _quizRepository = quizRepository;
        _itemRepository = itemRepository;
    }

    [HttpGet]
    public IEnumerable<QuizDto> GetAllQuizzes()
    {
        return _service.FindAllQuizzes().Select(QuizDto.Of);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<QuizDto> GetQuizById(int id)
    {
        var quiz = _service.FindQuizById(id);
        if (quiz == null) return NotFound();
        return Ok(QuizDto.Of(quiz));
    }

    [HttpPost]
    public ActionResult<QuizDto> CreateQuiz([FromBody] QuizCreateDto dto)
    {
        var quiz = new Quiz(
            id: _quizRepository.FindAll().Count() + 1,
            items: new List<QuizItem>(),
            title: dto.Title
        );
        _quizRepository.Add(quiz);
        return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Id }, QuizDto.Of(quiz));
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteQuiz(int id)
    {
        var quiz = _service.FindQuizById(id);
        if (quiz == null) return NotFound();
        _quizRepository.RemoveById(id);
        return NoContent();
    }

    [HttpPost]
    [Route("{quizId}/items")]
    public ActionResult<QuizItemDto> AddQuizItem(int quizId, [FromBody] QuizItemCreateDto dto)
    {
        var quiz = _service.FindQuizById(quizId);
        if (quiz == null) return NotFound();

        var item = new QuizItem(
            id: _itemRepository.FindAll().Count() + 1,
            question: dto.Question,
            incorrectAnswers: dto.IncorrectAnswers,
            correctAnswer: dto.CorrectAnswer
        );
        quiz.Items.Add(item);
        _itemRepository.Add(item);
        return CreatedAtAction(nameof(GetQuizById), new { id = quizId }, QuizItemDto.Of(item));
    }
}