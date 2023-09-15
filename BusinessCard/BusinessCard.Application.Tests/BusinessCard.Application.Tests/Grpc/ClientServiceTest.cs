using BusinessCard.API.Services;
using BusinessCard.Application.Application.Commands;
using BusinessCard.Application.Application.Commands.AddClient;
using BusinessCard.Application.Application.Commands.EditClient;
using BusinessCard.Application.Extensions;
using BusinessCard.GrpcServices.Services;
using ClientService;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessCard.Application.Tests_.Grpc;

public class ClientServiceTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ClientsService>> _loggerMock;
    private readonly ClientsService _clientsService;

    public ClientServiceTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ClientsService>>();
        _clientsService = new ClientsService(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddClientGrpc_ValidCommand_ReturnsClientCommandResult()
    {
        // Arrange
        var request = new AddClientGrpcCommand();
        var expectedResult = CommandResult.Success(Guid.NewGuid());
        
        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<AddClientCommand>(), CancellationToken.None))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _clientsService.AddClientGrpc(request, Mock.Of<ServerCallContext>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.Data.ToString(), result.ClientId);
    }
    
    [Fact]
    public async Task EditClientGrpc_ValidCommand_ReturnsClientCommandResult()
    {
        // Arrange
        var request = new EditClientGrpcCommand()
        {
            Id = Guid.NewGuid().ToString(),
            CompanyName = "SonicLynx",
            IsDiscreet = true,
            Subscription = 1
        };

        var expectedResult = request.Id.ToGuid();
        
        _mediatorMock
            .Setup( mediator => mediator.Send(It.IsAny<EditClientCommand>(), CancellationToken.None))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _clientsService.EditClientGrpc(request, Mock.Of<ServerCallContext>());

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(expectedResult.ToString(), result.ClientId);
    }

}