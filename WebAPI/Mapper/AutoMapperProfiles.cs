using ApplicationCore.Models;
using ApplicationCore.Models.QuizAggregate;
using AutoMapper;
using Dto;

namespace WebAPI.Mapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
      
        CreateMap<QuizItem, QuizItemDto>()
            .ForMember(
                dest => dest.Options,
                opt => opt.MapFrom(src => src.IncorrectAnswers.Concat(new[] { src.CorrectAnswer }).ToList()))
            .AfterMap((src, dest) =>
            {
                
                dest.Options = dest.Options.OrderBy(x => Guid.NewGuid()).ToList();
            });

        
        CreateMap<Quiz, QuizDto>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.Items));

        CreateMap<NewQuizDto, Quiz>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id is set by the service
            .ForMember(dest => dest.Items, opt => opt.Ignore()); // Items are added separately

        
        CreateMap<QuizItemUserAnswer, FeedbackDto.AnswerFeedbackDto>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.QuizItem.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect()));

      
        CreateMap<List<QuizItemUserAnswer>, FeedbackDto>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.QuizId, opt => opt.Ignore()) // Set manually in controller
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Set manually in controller
            .ForMember(dest => dest.TotalQuestions, opt => opt.Ignore()); // Set manually in controller
    }
}