using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using AWSLambda1.BookServices.Models;
using Newtonsoft.Json;
using JsonSerializer = Amazon.Lambda.Serialization.Json.JsonSerializer;

[assembly: LambdaSerializer(typeof(JsonSerializer))]
namespace AWSLambda1
{
    public class Authorizer
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public AuthPolicy Handler(APIGatewayCustomAuthorizeRequest authorizeRequest)
        {
            try
            {
                var token = authorizeRequest.AuthorizationToken;
                bool authorized = CheckAuthorization(token);

                var authPolicy = new AuthPolicy
                {
                    PrincipalId = token,
                    PolicyDocument = new PolicyDocument
                    {
                        Version = "2012-10-17",
                        Statement = new List<States>()
                    },
                    Context = new Context
                    {
                        Key = "string",
                        NumKey = 123,
                        BoolKey = true
                    }
                };

                if (authorized)
                {
                    var statement = new States
                    {
                        Action = "execute-api:Invoke",
                        Effect = "Allow",
                        Resource = authorizeRequest.MethodArn
                    };
                    authPolicy.PolicyDocument.Statement.Add(statement);
                }
                else
                {
                    var statement = new States
                    {
                        Action = "execute-api:Invoke",
                        Effect = "Deny",
                        Resource = authorizeRequest.MethodArn
                    };
                    authPolicy.PolicyDocument.Statement.Add(statement);
                }

                return authPolicy;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error authorizing request. " + e.Message);
                throw;
            }
        }

        private bool CheckAuthorization(string token)
        {
            if (token.ToLower().Equals("allow"))
            {
                return true;
            }

            if (token.ToLower().Equals("deny"))
            {
                return false;
            }

            throw new Exception("500");
        }
    }

    public class AuthPolicy
    {
        [JsonProperty("principalId")]
        public string PrincipalId { get; set; }

        [JsonProperty("policyDocument")]
        public PolicyDocument PolicyDocument { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; } 
    }

    public class Context
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("numKey")]
        public int NumKey { get; set; }

        [JsonProperty("boolKey")]
        public bool BoolKey { get; set; }
    }

    public class PolicyDocument
    {
        public string Version { get; set; }

        public List<States> Statement { get; set; }
    }

    public class States
    {
        public string Action { get; set; }

        public string Effect { get; set; }

        public string Resource { get; set; }
    }
}