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
            var newTierId = 1;
            var addedEntity = new Client (newName, newIsDiscreet, newTierId );

            mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(), It.IsAny<bool>(), 1)).ReturnsAsync(addedEntity);

            // Act
            var createdClient = await mockRepository.Object.CreateAsync(newName, newIsDiscreet, newTierId);

            mockRepository.Verify(c => c.CreateAsync("Test Company", true, 1));

            // Assert
            Assert.NotNull(createdClient);
            Assert.Equal(newName, createdClient.CompanyName);
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
            var newTierId = 1;
            var addedEntity = new Client (newName, newIsDiscreet, newTierId );

            mockRepository.Setup(c => c.CreateAsync(It.IsAny<string>(), It.IsAny<bool>(), 1)).ReturnsAsync(addedEntity);
            mockRepository.Setup(c => c.GetEntityByIdAsync(It.IsAny<Guid>())).ReturnsAsync(addedEntity);

            // Act
            var guid = Guid.NewGuid();
            await mockRepository.Object.CreateAsync(newName, newIsDiscreet, newTierId);
            var createdClient = await mockRepository.Object.GetEntityByIdAsync(guid);

            mockRepository.Verify(c => c.CreateAsync("Test Company", true, 1));
            mockRepository.Verify(c => c.GetEntityByIdAsync(guid));

            // Assert
            Assert.NotNull(createdClient);
            Assert.Equal(newName, createdClient.CompanyName);
            Assert.Equal(newIsDiscreet, createdClient.IsDiscreet);
            Assert.Equal(addedEntity.Id, createdClient.Id);
        }
    }
}
