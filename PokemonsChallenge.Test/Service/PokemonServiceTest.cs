using Moq;
using PokemonsChallenge.Service;
using PokemonsChallenge.Service.Interfaces;
using PokemonsChallenge.Repository.Interfaces;
using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PokemonsChallenge.Test.Service
{
    public class PokemonServiceTest
    {
        private readonly Mock<IPokemonRepository> _mockPokemonRepository;
        private readonly Mock<IPokeApiService> _mockPokeApiService;
        private readonly PokemonService _pokemonService;

        public PokemonServiceTest()
        {
            _mockPokemonRepository = new Mock<IPokemonRepository>();
            _mockPokeApiService = new Mock<IPokeApiService>();
            _pokemonService = new PokemonService(_mockPokemonRepository.Object, _mockPokeApiService.Object);
        }

        [Fact]
        public async Task CapturePokemonAsync_ShouldCapturePokemon_WhenPokemonExists()
        {
            // Arrange
            var trainerId = 1;
            var pokemonId = 25; // Pikachu
            var pokemonDto = new PokemonDto { Id = pokemonId, Name = "Pikachu" };

            _mockPokeApiService.Setup(s => s.GetPokemonByIdAsync(pokemonId)).ReturnsAsync(pokemonDto);
            _mockPokemonRepository.Setup(r => r.CapturePokemonAsync(trainerId, pokemonDto)).Returns(Task.CompletedTask);

            // Act
            await _pokemonService.CapturePokemonAsync(trainerId, pokemonId);

            // Assert
            _mockPokeApiService.Verify(s => s.GetPokemonByIdAsync(pokemonId), Times.Once);
            _mockPokemonRepository.Verify(r => r.CapturePokemonAsync(trainerId, pokemonDto), Times.Once);
        }

        [Fact]
        public async Task CapturePokemonAsync_ShouldThrowException_WhenPokemonNotFound()
        {
            // Arrange
            var trainerId = 1;
            var pokemonId = 999; // Pokémon não existente

            _mockPokeApiService.Setup(s => s.GetPokemonByIdAsync(pokemonId)).ReturnsAsync((PokemonDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _pokemonService.CapturePokemonAsync(trainerId, pokemonId));
        }

        [Fact]
        public async Task GetCapturedPokemonsAsync_ShouldReturnCapturedPokemons()
        {
            // Arrange
            var trainerId = 1;
            var capturedPokemons = new List<CapturedPokemon>
            {
                new CapturedPokemon { TrainerId = trainerId, PokemonId = 1 },
                new CapturedPokemon { TrainerId = trainerId, PokemonId = 2 }
            };

            _mockPokemonRepository.Setup(r => r.GetCapturedPokemonsAsync(trainerId)).ReturnsAsync(capturedPokemons);

            // Act
            var result = await _pokemonService.GetCapturedPokemonsAsync(trainerId);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetRandomPokemonsAsync_ShouldReturnListOfPokemons()
        {
            // Arrange
            var pokemonDto = new PokemonDto { Id = 1, Name = "Bulbasaur" };
            _mockPokeApiService.Setup(s => s.GetPokemonByIdAsync(It.IsAny<int>())).ReturnsAsync(pokemonDto);

            // Act
            var result = await _pokemonService.GetRandomPokemonsAsync();

            // Assert
            Assert.Equal(10, result.Count);
            _mockPokeApiService.Verify(s => s.GetPokemonByIdAsync(It.IsAny<int>()), Times.Exactly(10));
        }
    }
}
