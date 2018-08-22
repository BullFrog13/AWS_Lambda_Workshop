using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using AWSLambda1.BookServices.Models;

namespace AWSLambda1.BookServices
{
    public class GetBook
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<List<Book>> Handler(int id)
        {
            var response = await GetBookFromDynamoDb(id);

            return response;
        }

        private async Task<List<Book>> GetBookFromDynamoDb(string id)
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