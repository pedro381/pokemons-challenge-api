using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonsChallenge.Api.Controllers;
using PokemonsChallenge.Api.DTOs;
using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Service.Interfaces;
using PokemonsChallenge.Domain.Entities;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace PokemonsChallenge.Test.Api
{
    public class PokemonControllerTest
    {
        private readonly Mock<IPokemonService> _pokemonServiceMock;
        private readonly PokemonController _controller;

        public PokemonControllerTest()
        {
            _pokemonServiceMock = new Mock<IPokemonService>();
            _controller = new PokemonController(_pokemonServiceMock.Object);
        }

        [Fact]
        public async Task GetRandomPokemons_ReturnsOkResult_WithListOfPokemons()
        {
            // Arrange
            var pokemonList = new List<PokemonDto>
            {
                new PokemonDto { Id = 1, Name = "Pikachu" },
                new PokemonDto { Id = 2, Name = "Charmander" }
            };
            _pokemonServiceMock.Setup(s => s.GetRandomPokemonsAsync()).ReturnsAsync(pokemonList);

            // Act
            var result = await _controller.GetRandomPokemons();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pokemonList, okResult.Value);
        }

        [Fact]
        public async Task GetPokemonById_InvalidId_ReturnsBadRequest()
        {
            // Arrange: id inválido (<= 0)
            int invalidId = 0;

            // Act
            var result = await _controller.GetPokemonById(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Invalid Pokemon ID.", messageValue);
        }

        [Fact]
        public async Task GetPokemonById_PokemonNotFound_ReturnsNotFound()
        {
            // Arrange: id válido mas serviço retorna null
            int pokemonId = 999;
            _pokemonServiceMock.Setup(s => s.GetPokemonByIdAsync(pokemonId))
                                .ReturnsAsync((PokemonDto)null);

            // Act
            var result = await _controller.GetPokemonById(pokemonId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            dynamic response = notFoundResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal($"Pokemon with ID {pokemonId} not found.", messageValue);
        }

        [Fact]
        public async Task GetPokemonById_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange: id válido, mas o serviço lança exceção
            int pokemonId = 1;
            _pokemonServiceMock.Setup(s => s.GetPokemonByIdAsync(pokemonId))
                                .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.GetPokemonById(pokemonId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            dynamic response = objectResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("An error occurred while fetching the pokemon.", messageValue);
        }

        [Fact]
        public async Task CapturePokemon_InvalidInput_ReturnsBadRequest()
        {
            // Arrange: dados inválidos (TrainerId e PokemonId <= 0)
            var invalidDto = new CapturePokemonDto { TrainerId = 0, PokemonId = 0 };

            // Act
            var result = await _controller.CapturePokemon(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequestResult.Value;
            // Usando reflexão para obter a propriedade "message"
            var messageProperty = value.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(value, null)?.ToString();
            Assert.Equal("Invalid input data.", messageValue);
        }


        [Fact]
        public async Task CapturePokemon_ServiceThrowsInvalidOperationException_ReturnsBadRequest()
        {
            // Arrange: dados válidos, mas o serviço lança InvalidOperationException
            var validDto = new CapturePokemonDto { TrainerId = 1, PokemonId = 1 };
            _pokemonServiceMock.Setup(s => s.CapturePokemonAsync(validDto.TrainerId, validDto.PokemonId))
                                .ThrowsAsync(new InvalidOperationException("Pokemon with ID 1 not found."));

            // Act
            var result = await _controller.CapturePokemon(validDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Pokemon with ID 1 not found.", messageValue);
        }

        [Fact]
        public async Task CapturePokemon_Success_ReturnsOk()
        {
            // Arrange: dados válidos e serviço completa sem erro
            var validDto = new CapturePokemonDto { TrainerId = 1, PokemonId = 1 };
            _pokemonServiceMock.Setup(s => s.CapturePokemonAsync(validDto.TrainerId, validDto.PokemonId))
                                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CapturePokemon(validDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Pokemon captured successfully!", messageValue);
        }

        [Fact]
        public async Task GetCapturedPokemons_InvalidTrainerId_ReturnsBadRequest()
        {
            // Arrange: trainerId inválido (<= 0)
            int invalidTrainerId = 0;

            // Act
            var result = await _controller.GetCapturedPokemons(invalidTrainerId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic response = badRequestResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("Invalid Trainer ID.", messageValue);
        }

        [Fact]
        public async Task GetCapturedPokemons_Success_ReturnsOk()
        {
            // Arrange: trainerId válido e serviço retorna lista de capturados
            int trainerId = 1;
            var capturedList = new List<CapturedPokemon>
            {
                new CapturedPokemon { PokemonId = 1, CapturedAt = DateTime.UtcNow },
                new CapturedPokemon { PokemonId = 2, CapturedAt = DateTime.UtcNow }
            };
            _pokemonServiceMock.Setup(s => s.GetCapturedPokemonsAsync(trainerId))
                                .ReturnsAsync(capturedList);

            // Act
            var result = await _controller.GetCapturedPokemons(trainerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(capturedList, okResult.Value);
        }

        [Fact]
        public async Task GetCapturedPokemons_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange: trainerId válido, mas o serviço lança exceção
            int trainerId = 1;
            _pokemonServiceMock.Setup(s => s.GetCapturedPokemonsAsync(trainerId))
                                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetCapturedPokemons(trainerId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            dynamic response = objectResult.Value;
            var messageProperty = response.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            var messageValue = messageProperty.GetValue(response, null)?.ToString();
            Assert.Equal("An error occurred while fetching captured pokemons.", messageValue);
        }
    }
}
