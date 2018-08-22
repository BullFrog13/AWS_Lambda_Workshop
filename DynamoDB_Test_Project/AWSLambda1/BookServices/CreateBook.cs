using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using AWSLambda1.BookServices.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
// [assembly: LambdaSerializer(typeof(JsonSerializer))]
namespace AWSLambda1.BookServices
{
    public class CreateBook
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<HttpStatusCode> Handler(CreateBookRequest bookRequest)
        {
            var response = await Createbook(bookRequest);

            return response.HttpStatusCode;
        }

        private async Task<PutItemResponse> Createbook(CreateBookRequest bookRequest)
        {
            var dynamoDbClient = InitDynamoDbClient();
            var id = GenerateUniqueId();

            var request = new PutItemRequest
            {
                TableName = "Book",
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        "Id", new AttributeValue
                        {
                            N = id.ToString()
                        }
                    },
                    {
                        "Author", new AttributeValue
                        {
                            S = bookRequest.Author
                        }
                    },
                    { "Pages", new AttributeValue
                        {
                            N = bookRequest.Pages.ToString()
                        }
                    },
                    {
                        "Title", new AttributeValue
                        {
                            S = bookRequest.Title
                        }
                    },
                    {
                        "Publisher", new AttributeValue
                        {
                            S = bookRequest.Publisher
                        }
                    }
                }
            };

            return await dynamoDbClient.PutItemAsync(request);
        }

        private AmazonDynamoDBClient InitDynamoDbClient()
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest2
            };

            return new AmazonDynamoDBClient(clientConfig);
        }

        private uint GenerateUniqueId()
        {
            RandomNumberGenerator generator = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            generator.GetBytes(byteArray);

            return BitConverter.ToUInt32(byteArray, 0);
        }
    }
}