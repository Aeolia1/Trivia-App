using System;
namespace TriviaWebApp.Models
{
    public class Trivia
    {
        
        public int Id { get; set; }
        public string TriviaQuestion { get; set; }
        public string TriviaAnswer { get; set; }

        public Trivia()
        {
        }

    }
}

