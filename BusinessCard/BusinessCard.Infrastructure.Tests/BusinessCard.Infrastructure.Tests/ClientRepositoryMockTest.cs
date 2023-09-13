using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using BusinessCard.Domain.Seedwork;
using BusinessCard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BusinessCard.Infrastructure.Tests.Repositories
{
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
            var addedEntity = new Client (newName, newIsDiscreet, industry );

            mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(addedEntity);

            // Act
            var createdClient = await mockRepository.Object.CreateAsync(newName, newIsDiscreet,industry);

            mockRepository.Verify(c => c.CreateAsync("Test Company", true, industry));

            // Assert
            Assert.NotNull(createdClient);
            Assert.Equal(newName, createdClient.Name);
            Assert.Equal(newIsDiscreet, createdClient.IsDiscreet);
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
            var addedEntity = new Client (newName, newIsDiscreet, industry );

            mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(addedEntity);
            mockRepository.Setup(c => c.GetEntityByIdAsync(It.IsAny<Guid>())).ReturnsAsync(addedEntity);

            // Act
            var guid = Guid.NewGuid();
            await mockRepository.Object.CreateAsync(newName, newIsDiscreet, industry);
            var createdClient = await mockRepository.Object.GetEntityByIdAsync(guid);

            mockRepository.Verify(c => c.CreateAsync("Test Company", true, industry));
            mockRepository.Verify(c => c.GetEntityByIdAsync(guid));

            // Assert
            Assert.NotNull(createdClient);
            Assert.Equal(newName, createdClient.Name);
            Assert.Equal(newIsDiscreet, createdClient.IsDiscreet);
            Assert.Equal(addedEntity.Id, createdClient.Id);
        }
    }
}
