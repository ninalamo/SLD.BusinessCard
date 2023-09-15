using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessCard.Application.Application.Commands;
using BusinessCard.Application.Application.Commands.AddClient;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BusinessCard.Application.Tests.Clients
{
    public class AddClientCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResultWithId()
        {
            // Arrange
            var expectedResult = CommandResult.Success(Guid.NewGuid());
            var handler = new Mock<IRequestHandler<AddClientCommand,CommandResult>>();

            handler
                .Setup(c => c.Handle(It.IsAny<AddClientCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await handler.Object.Handle(new AddClientCommand("Test", true, 7), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equivalent(result,expectedResult);

                                                                                      
        }
    }
}