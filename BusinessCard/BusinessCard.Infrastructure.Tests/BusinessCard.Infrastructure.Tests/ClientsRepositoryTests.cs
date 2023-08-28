using System.Text.Json;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;

namespace BusinessCard.Infrastructure.Tests;

public class ClientsRepositoryTests
{
    private record SocialMediaObject
    {
        public string Facebook { get; init; }
        public string LinkedIn { get; init; }
        public string Instagram { get; init; }
        public string Pinterest { get; init; }
        public string Twitter { get; init; }
    }

    
    
    [Fact]
    public async Task GetEntityByIdAsync_ReturnsClient_WhenClientExists()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();

        var existingClient = new Client("Existing Client", true, 1);
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
    public async Task GetWithPropertiesByIdAsync_ShouldReturnClientWithProperties()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);

        
        // Act
        var client = await repository.CreateAsync("SonicLynx", true, 1);
        await repository.UnitOfWork.SaveChangesAsync(new CancellationToken());


        // Act
        var result = await repository.GetEntityByIdAsync(client.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(client.Id, result.Id);
        Assert.Equal(client.CompanyName, result.CompanyName);
    }

  
    
    [Fact]
    public async Task AddClientAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context =  await CreateInMemoryDbContext();
        var repository = new ClientsRepository(context);
        const string name = "New Client";

        // Act
        var client = await repository.CreateAsync(name, true, 1);
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
        var repository = new ClientsRepository(context);
        var client = await repository.CreateAsync("SonicLynxDigital", true, 7);
        await repository.UnitOfWork.SaveChangesAsync();
        var id = client.Id;
        client = null;

        // Act
        var clientToUpdate = await repository.GetWithPropertiesByIdAsync(id);
        clientToUpdate.Amend("Updated Client", false, 2);
        repository.Update(clientToUpdate);
        await repository.UnitOfWork.SaveChangesAsync(default);

        // Assert
        Assert.NotNull(clientToUpdate);
        Assert.Equal(id, clientToUpdate.Id);
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
        var json = new SocialMediaObject()
        {
            Facebook = "facebook.com",
            LinkedIn = "Linkedin.com",
            Instagram = "instagram.com",
            Pinterest = "pinterest.com",
            Twitter = "twitter.com",
        };
        var person = client.AddMemberAsync("Nin", "Alamo", "", "", "1234", "nin.alamo@outlook.com", "Cavite",
            "Encoder",
            JsonSerializer.Serialize(json));
        
        repository.Update(client);
     
         await repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);
        
        
         // Assert
         Assert.Equal(json,JsonSerializer.Deserialize<SocialMediaObject>(person.SocialMedia));
         Assert.NotEqual(Guid.Empty,person.Id);
         Assert.NotNull(client.Persons);
                                                                                                                                                                                                                                                                                               
        Assert.True(client.Persons.Any());
    }
    
    [Fact]
    public async Task UpdateMemberAsync_ReturnsId_AfterSaveChanges()
    {
        // Arrange
        var context = await CreateInMemoryDbContext();
        var guid = await context.Clients.AsNoTracking().Select(c => c.Id).FirstOrDefaultAsync();
        var repository = new ClientsRepository(context);
        var client = await repository.GetWithPropertiesByIdAsync(guid);
        var json = new SocialMediaObject()
        {
            Facebook = "facebook.com",
            LinkedIn = "Linkedin.com",
            Instagram = "instagram.com",
            Pinterest = "pinterest.com",
            Twitter = "twitter.com",
        };
        client.AddMemberAsync("Nin", "Alamo", "", "", "1234", "nin.alamo@outlook.com", "Cavite",
            "Encoder",
            JsonSerializer.Serialize(json));
        repository.Update(client);
        await repository.UnitOfWork.SaveChangesAsync(CancellationToken.None);
        client = null;
        
        //Act
        client = await repository.GetWithPropertiesByIdAsync(guid);
        var person = client.Persons.FirstOrDefault();
        
        
        // Assert
        Assert.Equal(json,JsonSerializer.Deserialize<SocialMediaObject>(person.SocialMedia));
        Assert.NotNull(client.Persons);
        Assert.True(client.Persons.Any());
        Assert.True(client.Persons.Any(i => i.Email == "nin.alamo@outlook.com"));
        Assert.True(client.Persons.Any(i => i.Id == person.Id));
        Assert.NotNull(person.Subscription);
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
        Assert.Equal("KMC",context.Clients.First().CompanyName);
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

        if (!context.Subscriptions.Any())
        {
            await context.Subscriptions.AddRangeAsync(MemberTier.GetLevels());
        }
        
        if (!context.Clients.Any())
            await context.Clients.AddAsync(new Client("KMC", true, 1));

        await context.SaveChangesAsync(default);
        
        return context;
    }
}