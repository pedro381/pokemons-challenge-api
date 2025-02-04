using Moq;
using Xunit;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Service.Interfaces;
using PokemonsChallenge.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using PokemonsChallenge.Service;

namespace PokemonsChallenge.Test.Service
{
    public class TrainerServiceTest
    {
        private readonly Mock<ITrainerRepository> _mockTrainerRepository;
        private readonly ITrainerService _trainerService;

        public TrainerServiceTest()
        {
            _mockTrainerRepository = new Mock<ITrainerRepository>();
            _trainerService = new TrainerService(_mockTrainerRepository.Object);
        }

        [Fact]
        public async Task RegisterTrainerAsync_ShouldRegisterTrainer_WhenValidTrainer()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Ash Ketchum" };

            _mockTrainerRepository.Setup(repo => repo.AddTrainerAsync(It.IsAny<Trainer>()))
                .ReturnsAsync(trainer);

            // Act
            var result = await _trainerService.RegisterTrainerAsync(trainer);

            // Assert
            Assert.Equal(trainer, result);
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ShouldReturnTrainer_WhenTrainerExists()
        {
            // Arrange
            var trainerId = 1;
            var trainer = new Trainer { Id = trainerId, Name = "Ash Ketchum" };

            _mockTrainerRepository.Setup(repo => repo.GetTrainerByIdAsync(trainerId))
                .ReturnsAsync(trainer);

            // Act
            var result = await _trainerService.GetTrainerByIdAsync(trainerId);

            // Assert
            Assert.Equal(trainerId, result.Id);
            Assert.Equal("Ash Ketchum", result.Name);
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ShouldThrowException_WhenTrainerNotFound()
        {
            // Arrange
            var trainerId = 999; // ID inválido
            _mockTrainerRepository.Setup(repo => repo.GetTrainerByIdAsync(trainerId))
                .ReturnsAsync((Trainer)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _trainerService.GetTrainerByIdAsync(trainerId));
        }

        [Fact]
        public async Task GetTrainerByIdAsync_ShouldThrowException_WhenIdIsInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _trainerService.GetTrainerByIdAsync(0));
        }

        [Fact]
        public async Task RegisterTrainerAsync_ShouldThrowException_WhenTrainerIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _trainerService.RegisterTrainerAsync(null));
        }
    }
}
