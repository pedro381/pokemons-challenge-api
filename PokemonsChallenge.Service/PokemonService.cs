using PokemonsChallenge.Service.Interfaces;
using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository.Interfaces;
using System;

namespace PokemonsChallenge.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokeApiService _pokeApiService;
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonService(IPokemonRepository pokemonRepository, IPokeApiService pokeApiService)
        {
            _pokeApiService = pokeApiService ?? throw new ArgumentNullException(nameof(pokeApiService));
            _pokemonRepository = pokemonRepository ?? throw new ArgumentNullException(nameof(pokemonRepository));
        }

        public async Task CapturePokemonAsync(int trainerId, int pokemonId)
        {
            if (trainerId <= 0)
                throw new ArgumentException("Trainer ID must be a positive integer.", nameof(trainerId));

            if (pokemonId <= 0)
                throw new ArgumentException("Pokemon ID must be a positive integer.", nameof(pokemonId));

            try
            {
                var pokemon = await GetPokemonByIdAsync(pokemonId);
                if (pokemon == null)
                    throw new InvalidOperationException($"Pokemon with ID {pokemonId} not found.");

                await _pokemonRepository.CapturePokemonAsync(trainerId, pokemon);
            }
            catch (Exception ex)
            {
                // Aqui, ao invés de lançar uma Exception genérica, vamos lançar a InvalidOperationException
                throw new InvalidOperationException($"An error occurred while capturing the Pokemon: {ex.Message}", ex);
            }
        }


        public async Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int trainerId)
        {
            if (trainerId <= 0)
                throw new ArgumentException("Trainer ID must be a positive integer.", nameof(trainerId));

            try
            {
                return await _pokemonRepository.GetCapturedPokemonsAsync(trainerId);
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new Exception($"An error occurred while retrieving captured Pokemons: {ex.Message}", ex);
            }
        }

        public async Task<PokemonDto> GetPokemonByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Pokemon ID must be a positive integer.", nameof(id));

            try
            {
                return await _pokeApiService.GetPokemonByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new Exception($"An error occurred while retrieving the Pokemon with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<List<PokemonDto>> GetRandomPokemonsAsync()
        {
            var random = new Random();
            var pokemons = new List<PokemonDto>();

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    int randomId = random.Next(1, 151);
                    var pokemon = await _pokeApiService.GetPokemonByIdAsync(randomId);
                    if (pokemon != null)
                    {
                        pokemons.Add(pokemon);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                throw new Exception("An error occurred while retrieving random Pokemons.", ex);
            }

            return pokemons;
        }
    }
}
