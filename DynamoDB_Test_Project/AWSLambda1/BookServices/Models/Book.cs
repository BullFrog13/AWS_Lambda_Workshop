using Amazon.DynamoDBv2.DataModel;

namespace AWSLambda1.BookServices.Models
{
    [DynamoDBTable("Book")]
    public class Book
    {
        [DynamoDBProperty]
        public ulong Id { get; set; }

        [DynamoDBProperty]
        public string Author { get; set; }

        [DynamoDBProperty]
        public int Pages { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Publisher { get; set; }
    }
}