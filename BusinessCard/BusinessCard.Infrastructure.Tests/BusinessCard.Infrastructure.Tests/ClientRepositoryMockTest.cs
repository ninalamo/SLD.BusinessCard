using BusinessCard.Domain.AggregatesModel.ClientAggregate;

namespace BusinessCard.Infrastructure.Tests;

public class ClientsRepositoryMOckTests
{
    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsCreatedClient()
    {
        // Arrange
        var mockRepository = new Mock<IClientsRepository>();
            
        var newName = "Test Company";
        var newIsDiscreet = true;
        var industry = "BPO";
        var addedEntity = new Client (newName, industry );

        mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(),  It.IsAny<string>())).ReturnsAsync(addedEntity);

        // Act
        var createdClient = await mockRepository.Object.CreateAsync(newName,industry);
      
        // Assert
        mockRepository.Verify(c => c.CreateAsync("Test Company", industry));

        Assert.NotNull(createdClient);
        Assert.Equal(newName, createdClient.Name);
        Assert.Equal(addedEntity.Id, createdClient.Id);
    }

    [Fact]
    public async Task GetEntityByIdAsync_ExistingId_ReturnsClient()
    {
        // Arrange
        var mockRepository = new Mock<IClientsRepository>();
            
        var newName = "Test Company";
        var newIsDiscreet = true;
        var industry = "BPO";
        var addedEntity = new Client (newName, industry );

        mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(addedEntity);
        mockRepository.Setup(c => c.GetEntityByIdAsync(It.IsAny<Guid>())).ReturnsAsync(addedEntity);

        // Act
        var guid = Guid.NewGuid();
        await mockRepository.Object.CreateAsync(newName, industry);
        var createdClient = await mockRepository.Object.GetEntityByIdAsync(guid);

        mockRepository.Verify(c => c.CreateAsync("Test Company", industry));
        mockRepository.Verify(c => c.GetEntityByIdAsync(guid));

        // Assert
        Assert.NotNull(createdClient);
        Assert.Equal(newName, createdClient.Name);
        Assert.Equal(addedEntity.Id, createdClient.Id);
    }
}