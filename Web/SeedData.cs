using ApplicationCore.Commons.Repository;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;

namespace Infrastructure.Memory;
public static class SeedData
{
    public static void Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var quizRepo = provider.GetService<IGenericRepository<Quiz, int>>();
            var quizItemRepo = provider.GetService<IGenericRepository<QuizItem, int>>();
            
            //TODO Utwórz trzy pytania typu QuizItem
            //TODO Dodaj je do quizItemRepo
            //TODO Utwórz obiekt klasy Quiz z kolekcją pytań dodanych do quizItemRepo
            //TODO Dodaj Quiz do quizRepo
            
            List<QuizItem> quizItems = new List<QuizItem>();
            
            quizItems.Add(quizItemRepo.Add(new QuizItem(id: 1, correctAnswer: "10", question: "5 + 5",
                incorrectAnswers: new List<string>() {"5", "15", "5"})));

            quizItems.Add(quizItemRepo.Add(new QuizItem(id: 2, correctAnswer: "3", question: "7 - 4",
                incorrectAnswers: new List<string>() {"-2", "2", "1"})));
            
            quizItems.Add(quizItemRepo.Add(new QuizItem(id: 3, correctAnswer: "4", question: "2 * 2",
                incorrectAnswers: new List<string>() {"3", "5", "8"})));

            quizRepo.Add(new Quiz(id: 1, items: quizItems, title: "Matematyka"));
            
            List<QuizItem> quizItems2 = new List<QuizItem>();
            
            quizItems2.Add(quizItemRepo.Add(new QuizItem(id: 1, correctAnswer: "Real Madryt", question: "Najlepszy klub piłkarski świata",
                incorrectAnswers: new List<string>() {"Barcelona", "Legia Warszawa", "Manchester United"})));
            
            quizItems2.Add(quizItemRepo.Add(new QuizItem(id: 2, correctAnswer: "Real Madryt", question: "Jaki klub wygrał lige mistrzów w sezonie 23/24",
                incorrectAnswers: new List<string>() {"Barcelona", "Manchester City", "Liverpool"})));
            quizItems2.Add(quizItemRepo.Add(new QuizItem(id: 3, correctAnswer: "15", question: "Ile lig mistrzów wygrał Real Madryt w swojej historii",
                incorrectAnswers: new List<string>() {"12", "5", "16"})));
            
            
            quizRepo.Add(new Quiz(id: 2, items: quizItems2, title: "Piłka nożna"));
            
            List<QuizItem> quizItems3 = new List<QuizItem>();
            
            quizItems3.Add(quizItemRepo.Add(new QuizItem(id: 1, correctAnswer: "Amazonka", question: "Najdłuższa rzeka świata",
                incorrectAnswers: new List<string>() {"Nil", "Wisła", "Dunajec"})));
            
            quizItems3.Add(quizItemRepo.Add(new QuizItem(id: 2, correctAnswer: "460", question: "Ilu jest posłów w polskim sejmie",
                incorrectAnswers: new List<string>() {"200", "500", "430"})));
            
            
            
            quizRepo.Add(new Quiz(id: 3, items: quizItems3, title: "Wiedza ogólna"));
        }
    }
}