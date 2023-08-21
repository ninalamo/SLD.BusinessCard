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
        var context = await CreateInMemoryDbContext();

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
        var context =  await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);
        const string name = "New Client";

        // Act
        var client = await repository.CreateAsync(name, true, Guid.NewGuid());
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
        var context =  await CreateInMemoryDbContext();

        var existingClient = new Client("Existing Client", true, 1);
        var existingClientId = context.Clients.Add(existingClient).Entity.Id;
        await context.SaveChangesAsync(); 

        var repository = new ClientsRepository(context);

        // Act
        var clientToUpdate = await repository.GetWithPropertiesByIdAsync(existingClientId);
        clientToUpdate.UpdateSelf("Updated Client", false, 2);
        repository.Update(clientToUpdate);
        await repository.UnitOfWork.SaveChangesAsync(default);

        // Assert
        Assert.NotNull(clientToUpdate);
        Assert.Equal(existingClientId, clientToUpdate.Id);
        Assert.Equal("Updated Client", clientToUpdate.CompanyName);
    }
    
    [Fact]
    public async Task AddMemberAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();
        var guid = await context.Clients.AsNoTracking().Select(c => c.Id).FirstOrDefaultAsync();
        var repository = new ClientsRepository(context);

        //Act
        var client = await repository.GetWithPropertiesByIdAsync(guid);
        var person =await client.AddMember("Nin", "Alamo", "", "", "1234", "nin.alamo@outlook.com", "Cavite", "Encoder",
            new[] { "facebook.com" });
        repository.Update(client);
     
         await repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);
        
        
         // Assert
         Assert.NotEqual(Guid.Empty,person.Id);
         Assert.NotNull(client.Persons);
                                                                                                                                                                                                                                                                                               
        Assert.True(client.Persons.Any());
    }



    private static async Task<LokiContext> CreateInMemoryDbContext()
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

        await context.Clients.AddAsync(new Client("KMC", true, 1));
        await context.SaveChangesAsync(default);
        
        return context;
    }
}