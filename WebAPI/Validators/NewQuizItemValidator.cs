using Dto;
using FluentValidation;

namespace WebAPI.Validators;

public class NewQuizItemValidator: AbstractValidator<NewQuizItemDto>
{
    public NewQuizItemValidator()
    {
        RuleFor(q => q.Question)
            .MaximumLength(200).WithMessage("Question cannot exceed 200 characters.")
            .MinimumLength(3).WithMessage("Question must be at least 3 characters long!");
        RuleForEach(q => q.Options)
            .MaximumLength(200)
            .MinimumLength(1);
        RuleFor(q => q.CorrectOptionIndex)
            .Must((dto, index) => index >= 0 && index < dto.Options.Count)
            .WithMessage("CorrectOptionIndex must be within the range of the options array.");
        RuleFor(q => q.Options)
            .Must(o => o.Count > 0).WithMessage("Options list cannot be empty.");
    }
}