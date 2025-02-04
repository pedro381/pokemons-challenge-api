using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository;
using PokemonsChallenge.Repository.Interfaces;
using Xunit;

namespace PokemonsChallenge.Test.Repository
{
    public class TrainerRepositoryTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ITrainerRepository _trainerRepository;

        public TrainerRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
                          .Options;

            _context = new ApplicationDbContext(options);
            _trainerRepository = new TrainerRepository(_context);
        }

        [Fact]
        public async Task AddTrainerAsync_ShouldThrowArgumentException_WhenTrainerIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _trainerRepository.AddTrainerAsync(null));
        }

        [Fact]
        public async Task AddTrainerAsync_ShouldSaveTrainer_WhenTrainerIsValid()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Ash", Age = 10, Cpf = "12345678901" };

            // Act
            var result = await _trainerRepository.AddTrainerAsync(trainer);

            // Assert
            Assert.Equal(trainer.Name, result.Name);
            Assert.Equal(trainer.Age, result.Age);
            Assert.Equal(trainer.Cpf, result.Cpf);
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ShouldThrowException_WhenTrainerNotFound()
        {
            // Arrange
            var trainerId = 1;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _trainerRepository.GetTrainerByIdAsync(trainerId));
            Assert.Contains($"Trainer with ID {trainerId} not found", exception.Message);
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ShouldReturnTrainer_WhenTrainerExists()
        {
            // Arrange
            var trainerId = 1;
            var trainer = new Trainer { Id = trainerId, Name = "Ash", Age = 10, Cpf = "12345678901" };

            // Add the trainer to the in-memory database
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _trainerRepository.GetTrainerByIdAsync(trainerId);

            // Assert
            Assert.Equal(trainer.Name, result.Name);
            Assert.Equal(trainer.Age, result.Age);
            Assert.Equal(trainer.Cpf, result.Cpf);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Ensures the database is deleted after each test
        }
    }
}
