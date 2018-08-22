using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using AWSLambda1.BookServices.Models;

namespace AWSLambda1.BookServices
{
    public class GetBooks
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<List<Book>> Handler()
        {
            var response = await GetBooksFromDynamoDb();

            return response;
        }

        private async Task<List<Book>> GetBooksFromDynamoDb()
        {
            var dynamoDbClient = InitDynamoDbClient();
            var context = new DynamoDBContext(dynamoDbClient);

            return await context.ScanAsync<Book>(new List<ScanCondition>()).GetRemainingAsync();
        }

        private AmazonDynamoDBClient InitDynamoDbClient()
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest2
            };

            return new AmazonDynamoDBClient(clientConfig);
        }
    }
}