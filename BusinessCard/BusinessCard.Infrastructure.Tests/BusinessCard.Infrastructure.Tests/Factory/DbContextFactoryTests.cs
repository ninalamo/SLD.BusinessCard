using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Shouldly;

namespace BusinessCard.Infrastructure.Tests.Factory;

public class DbContextFactoryTests
{
    
    [Fact]
    public void CreateDbContext_ReturnsLokiContextWithSqlServerOptions()
    {
        // Arrange
        var factory = new LokiContextDesignFactory();
        var args = new string[0]; // No need for args in this test

        // Act
        var context = factory.CreateDbContext(args);

        // Ensure the expected connection string
        var expectedConnectionString = "Server=localhost;Database=kardb;User Id=sa;Password=someThingComplicated1234;";

        // Assert
        context.ShouldNotBeNull();
        context.ShouldBeOfType<LokiContext>();
        context.Database.ShouldNotBeNull();
        context.Database.ShouldBeOfType<DatabaseFacade>();
        context.Database.GetDbConnection().ConnectionString.ShouldBe(expectedConnectionString);
    }
    
}