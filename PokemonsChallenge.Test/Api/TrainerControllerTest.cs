using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PokemonsChallenge.Api.Controllers;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Service.Interfaces;

namespace PokemonsChallenge.Test.Api
{
    public class TrainerControllerTest
    {
        private readonly Mock<ITrainerService> _trainerServiceMock;
        private readonly TrainerController _controller;

        public TrainerControllerTest()
        {
            _trainerServiceMock = new Mock<ITrainerService>();
            _controller = new TrainerController(_trainerServiceMock.Object);
        }

        [Fact]
        public async Task RegisterTrainer_NullTrainer_ReturnsBadRequest()
        {
            // Arrange
            Trainer trainer = null;

            // Act
            var result = await _controller.RegisterTrainer(trainer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Trainer data is required.", messageValue);
        }

        [Fact]
        public async Task RegisterTrainer_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Ash Ketchum" };
            _trainerServiceMock.Setup(s => s.RegisterTrainerAsync(trainer))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.RegisterTrainer(trainer);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            dynamic response = objectResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("An error occurred while registering the trainer.", messageValue);
        }

        [Fact]
        public async Task GetTrainerById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = 0;

            // Act
            var result = await _controller.GetTrainerById(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Invalid trainer ID.", messageValue);
        }

        [Fact]
        public async Task GetTrainerById_TrainerNotFound_ReturnsNotFound()
        {
            // Arrange
            int trainerId = 123;
            _trainerServiceMock.Setup(s => s.GetTrainerByIdAsync(trainerId))
                .ReturnsAsync((Trainer)null);

            // Act
            var result = await _controller.GetTrainerById(trainerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic response = notFoundResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal($"Trainer with ID {trainerId} not found.", messageValue);
        }

        [Fact]
        public async Task GetTrainerById_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            int trainerId = 1;
            _trainerServiceMock.Setup(s => s.GetTrainerByIdAsync(trainerId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTrainerById(trainerId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            dynamic response = objectResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("An error occurred while fetching the trainer.", messageValue);
        }
    }
}
