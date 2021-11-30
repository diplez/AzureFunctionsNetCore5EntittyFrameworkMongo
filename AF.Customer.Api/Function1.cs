using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Blog.Service.BlogApi.Application.Features.Posts.Commands.CreatePost;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

namespace AF.Customer.Api
{
    public class Function1
    {

        private IMediator Mediator;

        public Function1(IMediator mediator)
        {
            Mediator = mediator;
        }

        [Function("Function1")]
        [OpenApiOperation(operationId: "addPet", tags: new[] { "pet" }, Summary = "Add a new pet to the store", Description = "This add a new pet to the store.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Summary = "New pet details added", Description = "New pet details added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]

        public async Task<HttpResponseData>Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var result1 = await Mediator.Send(new CreatePostCommand
            {
                CreatePostDto = new CreatePostDto(new CreatePostDto() { 
                    Content = "diego",
                    Uploads = new List<string>(),
                    UserId = "testDiego"
                    
                }, "dsa")
            });

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
