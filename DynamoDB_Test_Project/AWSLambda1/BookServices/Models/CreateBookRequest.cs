namespace AWSLambda1.BookServices.Models
{
    public class CreateBookRequest
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }
        
        public string Title { get; set; }

        public string Publisher { get; set; }
    }
}