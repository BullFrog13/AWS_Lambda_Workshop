namespace AWSLambda1.BookServices.Models
{
    public class APIGatewayCustomAuthorizeRequest
    {
        public string Type { get; set; }

        public string MethodArn { get; set; }

        public string AuthorizationToken { get; set; }
    }
}