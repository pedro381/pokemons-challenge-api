using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository;
using Microsoft.EntityFrameworkCore;

namespace PokemonsChallenge.Test.Repository
{
    public class PokemonRepositoryTest
    {
        private readonly PokemonRepository _repository;
        private readonly ApplicationDbContext _context;

        public PokemonRepositoryTest()
        {
            // Configurando o banco de dados em memória para simular o contexto
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "PokemonsChallengeTestDb")
                .Options;

            _context = new ApplicationDbContext(options);

            // Populando o banco de dados com dados de exemplo
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new PokemonRepository(_context);
        }

        [Fact]
        public async Task CapturePokemonAsync_ShouldCapturePokemon_WhenTrainerAndPokemonExist()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Pedro", Age = 28, Cpf = "12134408669" };
            _context.Trainers.Add(trainer);
            _context.SaveChanges();

            var pokemonDto = new PokemonDto
            {
                Id = 1,
                Name = "Pikachu",
                PokemonImageDto = new PokemonImageDto { Url = "https://image.url" }
            };

            var pokemon = new Pokemon
            {
                Id = pokemonDto.Id,
                Name = pokemonDto.Name,
                ImageUrl = pokemonDto.PokemonImageDto.Url
            };
            _context.Pokemons.Add(pokemon);
            _context.SaveChanges();

            // Act
            await _repository.CapturePokemonAsync(trainer.Id, pokemonDto);

            // Assert
            var capturedPokemon = await _context.CapturedPokemons
                                                 .Where(cp => cp.TrainerId == trainer.Id)
                                                 .Include(cp => cp.Pokemon)
                                                 .FirstOrDefaultAsync();

            Assert.NotNull(capturedPokemon);
            Assert.Equal(trainer.Id, capturedPokemon.TrainerId);
            Assert.Equal(pokemonDto.Id, capturedPokemon.PokemonId);
            Assert.Equal(pokemonDto.Name, capturedPokemon.Pokemon.Name);
        }

        [Fact]
        public async Task CapturePokemonAsync_ShouldThrowException_WhenTrainerNotFound()
        {
            // Arrange
            var pokemonDto = new PokemonDto
            {
                Id = 1,
                Name = "Pikachu",
                PokemonImageDto = new PokemonImageDto { Url = "https://image.url" }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _repository.CapturePokemonAsync(999, pokemonDto));  // TrainerId 999 não existe
            Assert.Contains("Trainer not found", exception.Message);
        }

        [Fact]
        public async Task GetCapturedPokemonsAsync_ShouldReturnCapturedPokemons_WhenPokemonsAreCaptured()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Pedro", Age = 28, Cpf = "12134408669" };
            _context.Trainers.Add(trainer);
            _context.SaveChanges();

            var pokemonDto = new PokemonDto
            {
                Id = 1,
                Name = "Pikachu",
                PokemonImageDto = new PokemonImageDto { Url = "https://image.url" }
            };

            var pokemon = new Pokemon
            {
                Id = pokemonDto.Id,
                Name = pokemonDto.Name,
                ImageUrl = pokemonDto.PokemonImageDto.Url
            };
            _context.Pokemons.Add(pokemon);
            _context.SaveChanges();

            // Capture the Pokémon
            await _repository.CapturePokemonAsync(trainer.Id, pokemonDto);

            // Act
            var capturedPokemons = await _repository.GetCapturedPokemonsAsync(trainer.Id);

            // Assert
            Assert.NotNull(capturedPokemons);
            Assert.Single(capturedPokemons);
            Assert.Equal(pokemonDto.Name, capturedPokemons.First().Pokemon.Name);
        }

        [Fact]
        public async Task GetCapturedPokemonsAsync_ShouldThrowException_WhenTrainerNotFound()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _repository.GetCapturedPokemonsAsync(999));  // TrainerId 999 não existe
            Assert.Contains("Trainer not found", exception.Message);
        }

        [Fact]
        public async Task GetCapturedPokemonsAsync_ShouldReturnEmptyList_WhenNoPokemonCaptured()
        {
            // Arrange
            var trainer = new Trainer { Id = 1, Name = "Pedro", Age = 28, Cpf = "12134408669" };
            _context.Trainers.Add(trainer);
            _context.SaveChanges();

            // Act
            var capturedPokemons = await _repository.GetCapturedPokemonsAsync(trainer.Id);

            // Assert
            Assert.NotNull(capturedPokemons);
            Assert.Empty(capturedPokemons);
        }
    }
}
