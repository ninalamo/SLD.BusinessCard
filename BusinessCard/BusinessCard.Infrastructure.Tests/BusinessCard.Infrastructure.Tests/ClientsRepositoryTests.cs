using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessCard.Infrastructure.Tests;

public class ClientsRepositoryTests
{

    [Fact]
    public async Task GetEntityByIdAsync_ReturnsClient_WhenClientExists()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();

        var existingClient = new Client("Existing Client", "");
        var existingClientId = context.Clients.Add(existingClient).Entity.Id;
        await context.SaveChangesAsync(); 

        var repository = new ClientsRepository(context);

        // Act
        var result = await repository.GetEntityByIdAsync(existingClientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingClientId, result.Id);
        Assert.Equal("Existing Client", result.Name);
    }
    
    [Fact]
    public async Task GetWithPropertiesByIdAsync_ShouldReturnClientWithProperties()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);

        
        // Act
        var client = await repository.CreateAsync("SonicLynx", "BPO");
        await repository.UnitOfWork.SaveChangesAsync(new CancellationToken());


        // Act
        var result = await repository.GetEntityByIdAsync(client.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(client.Id, result.Id);
        Assert.Equal(client.Name, result.Name);
    }

  
    
    [Fact]
    public async Task AddClientAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context =  await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);
        const string name = "New Client";

        // Act
        var client = await repository.CreateAsync(name, "");
        await repository.UnitOfWork.SaveChangesAsync(new CancellationToken());

        // Assert
        Assert.NotNull(client);
        Assert.NotEqual(client.Id, Guid.Empty);
        Assert.Equal(name,client.Name);
    }
    
    [Fact]
    public async Task UpdateClientAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context =  await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);
        var client = await repository.CreateAsync("SonicLynxDigital", "");
        await repository.UnitOfWork.SaveChangesAsync();
        var id = client.Id;

        // Act
        var clientToUpdate = await repository.GetWithPropertiesByIdAsync(id);
        clientToUpdate.Name = "Sonic Lynx";
        clientToUpdate.Industry = "Industry";
        
        repository.Update(clientToUpdate);
        await repository.UnitOfWork.SaveChangesAsync(default);

        // Assert
        Assert.NotNull(clientToUpdate);
        Assert.Equal(id, clientToUpdate.Id);
        Assert.Equal("Sonic Lynx", clientToUpdate.Name);
    }
 

    [Fact]
    public void TestCurrentUser()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Email).Returns("testing@soniclynx.digital");
        currentUserMock.Setup(x => x.Name).Returns("Testing Account");
        currentUserMock.Setup(x => x.IdentityId).Returns(Guid.NewGuid().ToString());
        currentUserMock.Setup(x => x.Roles).Returns(new[]{"admin"});

        var roles = currentUserMock.Object.Roles;
        Assert.Equal( "admin",roles[0]);
    }

    [Fact]
    public async Task TestDbContext()
    {
        // Arrange
        var context =  await CreateInMemoryDbContext();

        //Assert
        Assert.NotNull(context);
        Assert.NotNull(context.Clients);
        Assert.Equal("KMC",context.Clients.First().Name);
        Assert.True(context.Clients.First().Id != Guid.Empty);
    }

    private async Task<LokiContext> CreateInMemoryDbContext()
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
        
        if (!context.Clients.Any())
            await context.Clients.AddAsync(new Client("KMC", "Real Estate"));

        await context.SaveChangesAsync(default);
        
        return context;
    }
}