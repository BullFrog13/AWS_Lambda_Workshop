using Amazon.DynamoDBv2.DataModel;

namespace AWSLambda1.BookServices.Models
{
    [DynamoDBTable("Book")]
    public class Book
    {
        [DynamoDBHashKey]
        public int Id { get; set; }

        [DynamoDBProperty("Author")]
        public string Author { get; set; }

        [DynamoDBProperty("Pages")]
        public int Pages { get; set; }

        [DynamoDBProperty("Title")]
        public string Title { get; set; }

        [DynamoDBProperty("Publisher")]
        public string Publisher { get; set; }
    }
}