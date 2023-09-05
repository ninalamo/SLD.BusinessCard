// using BusinessCard.Application.Application.Behaviors;
// using BusinessCard.Application.Extensions;
// using BusinessCard.Application.Logging;
// using MediatR;
// using Microsoft.Extensions.Logging;
// using Moq;
//
// namespace BusinessCard.Application.Tests_;
//
// public class LoggingBehaviorTests
// {
//     [Fact]
//     public async Task Handle_LogsCommandHandlingAndHandled()
//     {
//         // Arrange
//         var loggerMock = new Mock<ILoggerAdapter<LoggingBehavior<SampleRequest, SampleResponse>>>();
//         var request = new SampleRequest();
//         var nextHandlerMock = new Mock<RequestHandlerDelegate<SampleResponse>>();
//         nextHandlerMock.Setup(h => h()).ReturnsAsync(new SampleResponse());
//
//         var behavior = new LoggingBehavior<SampleRequest, SampleResponse>(loggerMock.Object);
//
//         // Act
//         var result = await behavior.Handle(request, nextHandlerMock.Object, CancellationToken.None);
//
//         // Assert
//         Assert.NotNull(result);
//
//         loggerMock.Verify(
//             x => x.LogInformation("----- Handling command {CommandName} ({@Command})", 
//                 request.GetGenericTypeName(), request), 
//             Times.Once);
//
//         loggerMock.Verify(
//             x => x.LogInformation("----- Command {CommandName} handled - response: {@Response}",
//                 request.GetGenericTypeName(), result),
//             Times.Once);
//
//         nextHandlerMock.Verify(h => h(), Times.Once);
//     }
//
//
//
//     // Sample request and response classes for testing
//     public class SampleRequest : IRequest<SampleResponse>
//     {
//         public string Request => "request";
//     }
//
//     public class SampleResponse
//     {
//         public string Result => "result";
//     }
//
// }
