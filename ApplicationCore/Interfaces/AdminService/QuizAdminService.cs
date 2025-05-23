﻿using ApplicationCore.Commons.Repository;
using ApplicationCore.Models.QuizAggregate;

namespace BackendLab01;

public class QuizAdminService:IQuizAdminService
{
    private IGenericRepository<Quiz, int> quizRepository;
    private IGenericRepository<QuizItem, int> itemRepository;

    public QuizAdminService(IGenericRepository<Quiz, int> quizRepository, IGenericRepository<QuizItem, int> itemRepository)
    {
        this.quizRepository = quizRepository;
        this.itemRepository = itemRepository;
    }

    public QuizItem AddQuizItem(string question, List<string> incorrectAnswers, string correctAnswer, int points)
    {
        return itemRepository.Add(new QuizItem(question: question, incorrectAnswers: incorrectAnswers, correctAnswer: correctAnswer, id: 0));
    }

    public void UpdateQuizItem(int id, string question, List<string> incorrectAnswers, string correctAnswer, int points)
    {
        var quizItem = new QuizItem(id: id, question: question, incorrectAnswers: incorrectAnswers, correctAnswer: correctAnswer);
        itemRepository.Update(id, quizItem);
    }

    public Quiz AddQuiz(string title, List<QuizItem> items)
    {
        return quizRepository.Add(new Quiz( 0, title: title, items: items));
    }

    public List<QuizItem> FindAllQuizItems()
    {
        return itemRepository.FindAll();
    }

    public List<Quiz> FindAllQuizzes()
    { return quizRepository.FindAll();
    }
    public void AddQuizItemToQuiz(int quizId, QuizItem item)
    {
        var quiz = quizRepository.FindAll().FirstOrDefault(q => q.Id == quizId);
        if (quiz != null)
        {
            quiz.Items.Add(item);
            itemRepository.Add(item);
        }
    }
    
    public void DeleteQuiz(int quizId)
    {
        quizRepository.RemoveById(quizId);
    }

    public void UpdateQuiz(int quizId, Quiz quiz)
    {
        quizRepository.Update(quizId, quiz);
    }
}