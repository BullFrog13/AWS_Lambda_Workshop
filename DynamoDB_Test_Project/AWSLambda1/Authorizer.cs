using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using AWSLambda1.BookServices.Models;

namespace AWSLambda1
{
    public class Authorizer
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<AuthPolicy> Handler(APIGatewayCustomAuthorizeRequest authorizeRequest)
        {
            try
            {
                var token = authorizeRequest.AuthToken;
                bool authorized = CheckAuthorization(token);

                var authPolicy = new AuthPolicy
                {
                    PrincipalId = token,
                    PolicyStatement = new PolicyStatement
                    {
                        Version = "2012-10-17",
                        Statement = new List<States>()
                    }
                };

                if (authorized)
                {
                    var statement = new States
                    {
                        Action = "execute-api:Invoke",
                        Effect = "Allow",
                        Resource = "arn"
                    };
                    authPolicy.PolicyStatement.Statement.Add(statement);
                }
                else
                {
                    var statement = new States
                    {
                        Action = "execute-api:Invoke",
                        Effect = "Deny",
                        Resource = "arn"
                    };

                    authPolicy.PolicyStatement.Statement.Add(statement);
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
        public string PrincipalId { get; set; }

        public PolicyStatement PolicyStatement { get; set; }

        public Context Context { get; set; } 
    }

    public class Context
    {
        public string stringKey { get; set; }

        public int numberKey { get; set; }

        public bool booleanKey { get; set; }
    }

    public class PolicyStatement
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