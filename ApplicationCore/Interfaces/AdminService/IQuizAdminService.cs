﻿using ApplicationCore.Models.QuizAggregate;

namespace BackendLab01;

public interface IQuizAdminService
{
    public QuizItem AddQuizItem(string question, List<string> incorrectAnswers, string correctAnswer, int points);
    public void UpdateQuizItem(int id, string question, List<string> incorrectAnswers, string correctAnswer, int points);
    public Quiz AddQuiz(string title, List<QuizItem> items);
    public List<QuizItem> FindAllQuizItems();
    public List<Quiz> FindAllQuizzes();
    public void AddQuizItemToQuiz(int quizId, QuizItem item);
    public void DeleteQuiz(int quizId);
    public void UpdateQuiz(int quizId, Quiz quiz);
}