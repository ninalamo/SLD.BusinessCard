using BusinessCard.API.Application.Behaviors;
using BusinessCard.API.Application.Commands;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace BusinessCard.Application.Tests_;

public class FluentValidationTests
{
    [Fact]
    public async Task Handle_ValidRequest_NoValidationErrors()
    {
        // Arrange
        var validators = new List<IValidator<UpsertClientCommand>>
        {
            new UpsertClientCommandValidation()
        };

        var loggerMock = new Mock<ILogger<ValidatorBehavior<UpsertClientCommand, CommandResult>>>();
        var request = new UpsertClientCommand(null,"Sonic Lynx Digital",true,1);
        var nextHandlerMock = new Mock<RequestHandlerDelegate<CommandResult>>();
        nextHandlerMock.Setup(h => h()).ReturnsAsync(CommandResult.Success(Guid.NewGuid()));

        var behavior = new ValidatorBehavior<UpsertClientCommand, CommandResult>(validators, loggerMock.Object);

        // Act
        var result = await behavior.Handle(request, nextHandlerMock.Object, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        nextHandlerMock.Verify(h => h(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var validators = new List<IValidator<SampleRequest>>
        {
            new SampleRequestValidator()
        };

        var loggerMock = new Mock<ILogger<ValidatorBehavior<SampleRequest, SampleResponse>>>();
        var request = new SampleRequest(); // Invalid request
        var nextHandlerMock = new Mock<RequestHandlerDelegate<SampleResponse>>();
        nextHandlerMock.Setup(h => h()).ReturnsAsync(new SampleResponse());

        var behavior = new ValidatorBehavior<SampleRequest, SampleResponse>(validators, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessCardDomainException>(() =>
            behavior.Handle(request, nextHandlerMock.Object, CancellationToken.None));
        nextHandlerMock.Verify(h => h(), Times.Never);
    }
    
    // Sample request and response classes for testing
    public class SampleRequest : IRequest<SampleResponse>
    {
        public string Property { get; set; }
    }
    public class SampleResponse { }

    // Sample request validator for testing
    public class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
        {
            RuleFor(request => request.Property).NotNull();
        }
    }

}