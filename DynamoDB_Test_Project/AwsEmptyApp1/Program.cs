using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AWSLambda1.BookServices.Models;

// Add using statements to access AWS SDK for .NET services. 
// Both the Service and its Model namespace need to be added 
// in order to gain access to a service. For example, to access
// the EC2 service, add:
// using Amazon.EC2;
// using Amazon.EC2.Model;

namespace AwsEmptyApp1
{
    class Program
    {
        private static AmazonDynamoDBClient _dynamoDbClient;

        public static void Main(string[] args)
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.EUWest2
            };

            _dynamoDbClient = new AmazonDynamoDBClient(clientConfig);

            ListAllTables();

            GetBooksFromDynamoDb();

            //CreateItem();

            Console.Read();
        }

        private static void ListAllTables()
        {
            // Initial value for the first page of table names.
            string lastEvaluatedTableName = null;
            do
            {
                // Create a request object to specify optional parameters.
                var request = new ListTablesRequest
                {
                    Limit = 10, // Page size.
                    ExclusiveStartTableName = lastEvaluatedTableName
                };

                var response = _dynamoDbClient.ListTables(request);
                var result = response.TableNames;
                foreach (string name in result)
                    Console.WriteLine(name);

                lastEvaluatedTableName = response.LastEvaluatedTableName;

            } while (lastEvaluatedTableName != null);
        }

        private static void CreateItem()
        {
            RandomNumberGenerator generator = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            generator.GetBytes(byteArray);
            var id = BitConverter.ToUInt32(byteArray, 0);

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
                            S = "Gregory David Roberts"
                        }
                    },
                    { "Pages", new AttributeValue
                        {
                            N = "745"
                        }
                    },
                    {
                        "Title", new AttributeValue
                        {
                            S = "Shantaram"
                        }
                    },
                    {
                        "Publisher", new AttributeValue
                        {
                            S = "New York Times Publisher"
                        }
                    }
                }
            };

            var response = _dynamoDbClient.PutItem(request);
        }

        private static void GetBooksFromDynamoDb()
        {
            /* DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);
             List<Book> books = context.Scan<Book>().ToList();*/

            DynamoDBContext context = new DynamoDBContext(_dynamoDbClient);

            var test = context.ScanAsync<Book>(new List<ScanCondition>()).GetRemainingAsync().Result;
        }

        private static void PrintItem(Dictionary<string, AttributeValue> attributeList)
        {
            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {
                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;

                Console.WriteLine(
                    attributeName + " " +
                    (value.S == null ? "" : "S=[" + value.S + "]") +
                    (value.N == null ? "" : "N=[" + value.N + "]") +
                    (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
                    (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]")
                );
            }

            Console.WriteLine("************************************************");
        }
    }
}