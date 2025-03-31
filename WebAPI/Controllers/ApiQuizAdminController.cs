using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using BackendLab01;
using Dto;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/admin/quizzes")]
public class ApiQuizAdminController : ControllerBase
{
    private readonly IQuizAdminService _service;
    private readonly IMapper _mapper;

    public ApiQuizAdminController(IQuizAdminService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult<object> AddQuiz([FromServices] LinkGenerator link, [FromBody] NewQuizDto dto)
    {
        var quiz = _service.AddQuiz(_mapper.Map<Quiz>(dto).Title, new List<QuizItem>());
        return Created(
            link.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quizId = quiz.Id }),
            quiz
        );
    }

    [HttpGet]
    [Route("{quizId}")]
    public ActionResult<Quiz> GetQuiz(int quizId)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        return quiz is null ? NotFound() : Ok(quiz);
    }
    
    [HttpPost]
    [Route("{quizId}/items")]
    public ActionResult<QuizItemDto> AddQuizItem(int quizId, [FromBody] NewQuizItemDto dto, [FromServices] IValidator<NewQuizItemDto> validator)
    {
        var validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null) return NotFound();

        var correctAnswer = dto.Options[dto.CorrectOptionIndex];
        var incorrectAnswers = dto.Options.Where((_, i) => i != dto.CorrectOptionIndex).ToList();
        var item = _service.AddQuizItem(dto.Question, incorrectAnswers, correctAnswer, 1);

        quiz.Items.Add(item);
        return CreatedAtAction(nameof(GetQuiz), new { quizId = quizId }, QuizItemDto.Of(item));
    }
    [HttpDelete]
    [Route("{quizId}")]
    public IActionResult DeleteQuiz(int quizId)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null) return NotFound();

       
        if (_service.FindAllQuizzes().Any(q => q.Id == quizId && q.Items.Any(i => /* check for answers */ false)))
            return BadRequest("Cannot delete quiz with existing answers");

        _service.DeleteQuiz(quizId);
        return NoContent();
    }
    
    [HttpPut]
    [Route("{quizId}")]
    public ActionResult<Quiz> UpdateQuiz(int quizId, [FromBody] NewQuizDto dto)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null) return NotFound();

        quiz.Title = dto.Title;
        _service.UpdateQuiz(quizId, quiz);
        return Ok(quiz);
    }
    
    [HttpGet]
    [Route("{quizId}/items/{itemId}")]
    public ActionResult<QuizItem> GetQuizItem(int quizId, int itemId)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null) return NotFound("Quiz not found");

        var item = quiz.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return NotFound("Item not found");

        return Ok(item);
    }
    [HttpPatch]
    [Route("{quizId}")]
    [Consumes("application/json-patch+json")]
    public ActionResult<Quiz> AddQuizItem(int quizId, [FromBody] JsonPatchDocument<Quiz> patchDoc)
    {
        var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
        if (quiz is null || patchDoc is null)
        {
            return NotFound(new { error = $"Quiz with id {quizId} not found" });
        }

        int previousCount = quiz.Items.Count;
        patchDoc.ApplyTo(quiz, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (previousCount < quiz.Items.Count)
        {
            QuizItem item = quiz.Items[^1];
            quiz.Items.RemoveAt(quiz.Items.Count - 1);
            _service.AddQuizItemToQuiz(quizId, item);
        }

        return Ok(_service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
    }
}