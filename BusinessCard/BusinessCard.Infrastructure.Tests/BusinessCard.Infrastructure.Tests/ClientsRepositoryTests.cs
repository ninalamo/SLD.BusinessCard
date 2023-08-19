using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BusinessCard.Infrastructure.Tests;

public class ClientsRepositoryTests
{
    [Fact]
    public async Task GetEntityByIdAsync_ReturnsClient_WhenClientExists()
    {
        // Arrange
        var context = CreateInMemoryDbContext();

        var existingClient = new Client("Existing Client", true, Guid.NewGuid());
        var existingClientId = context.Clients.Add(existingClient).Entity.Id;
        await context.SaveChangesAsync(); 

        var repository = new ClientsRepository(context);

        // Act
        var result = await repository.GetEntityByIdAsync(existingClientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingClientId, result.Id);
        Assert.Equal("Existing Client", result.CompanyName);
    }
    
    [Fact]
    public async Task AddClientAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context = CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);
        const string name = "New Client";

        // Act
        var client = repository.Create(name, true, Guid.NewGuid());
        await repository.UnitOfWork.SaveChangesAsync(new CancellationToken());

        // Assert
        Assert.NotNull(client);
        Assert.NotEqual(client.Id, Guid.Empty);
        Assert.Equal(name,client.CompanyName);
    }
    
    [Fact]
    public async Task UpdateClientAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context = CreateInMemoryDbContext();

        var existingClient = new Client("Existing Client", true, 1);
        var existingClientId = context.Clients.Add(existingClient).Entity.Id;
        await context.SaveChangesAsync(); 

        var repository = new ClientsRepository(context);

        // Act
        var clientToUpdate = await repository.GetEntityByIdAsync(existingClientId);
        clientToUpdate.UpdateSelf("Updated Client", false, 2);
        repository.Update(clientToUpdate);
        await repository.UnitOfWork.SaveChangesAsync(default);

        // Assert
        Assert.NotNull(clientToUpdate);
        Assert.Equal(existingClientId, clientToUpdate.Id);
        Assert.Equal("Updated Client", clientToUpdate.CompanyName);
    }



    private static LokiContext CreateInMemoryDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<LokiContext>()
            .UseInMemoryDatabase(databaseName: "in-memory")
            .Options;

        var mediatorMock = new Mock<IMediator>();
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Email).Returns("testing@soniclynx.digital");
        currentUserMock.Setup(x => x.Name).Returns("Testing Account");
        currentUserMock.Setup(x => x.IdentityId).Returns(Guid.NewGuid().ToString());
        currentUserMock.Setup(x => x.Roles).Returns(new[]{"admin"});

        var context = new LokiContext(dbContextOptions, mediatorMock.Object, currentUserMock.Object);
        return context;
    }
}