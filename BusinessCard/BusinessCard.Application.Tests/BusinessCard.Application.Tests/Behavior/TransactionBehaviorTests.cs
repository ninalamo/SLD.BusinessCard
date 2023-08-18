using BusinessCard.API.Application.Behaviors;
using BusinessCard.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessCard.Application.Tests_;

public class TransactionBehaviourTests
{
    [Fact]
    public async Task Handle_CommitsTransactionOnSuccess()
    {
        // Arrange
        var dbContextMock = new Mock<LokiContext>();
        dbContextMock.Setup(db => db.Database.CreateExecutionStrategy())
            .Returns(MockExecutionStrategy());
            
        var loggerMock = new Mock<ILogger<TransactionBehaviour<SampleRequest, SampleResponse>>>();
        var request = new SampleRequest();
        var nextHandlerMock = new Mock<RequestHandlerDelegate<SampleResponse>>();
        nextHandlerMock.Setup(h => h()).ReturnsAsync(new SampleResponse());

        var behavior = new TransactionBehaviour<SampleRequest, SampleResponse>(
            dbContextMock.Object,
            loggerMock.Object
        );

        // Act
        var result = await behavior.Handle(request, nextHandlerMock.Object, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        nextHandlerMock.Verify(h => h(), Times.Once);
        dbContextMock.Verify(db => db.BeginTransactionAsync(), Times.Once);
        dbContextMock.Verify(db => db.CommitTransactionAsync(It.IsAny<IDbContextTransaction>()), Times.Once);
    }

    // Helper method to mock ExecutionStrategy
    private IExecutionStrategy MockExecutionStrategy()
    {
        var executionStrategyMock = new Mock<IExecutionStrategy>();
        executionStrategyMock.Setup(e => e.ExecuteAsync(It.IsAny<Func<Task>>()))
            .Returns<Func<Task>>(func => func.Invoke());
        return executionStrategyMock.Object;
    }
}

// Sample request and response classes for testing
public class SampleRequest : IRequest<SampleResponse> { }
public class SampleResponse { }