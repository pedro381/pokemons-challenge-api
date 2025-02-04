using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;

namespace PokemonsChallenge.Service.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonDto> GetPokemonByIdAsync(int id);
        Task<List<PokemonDto>> GetRandomPokemonsAsync();
        Task CapturePokemonAsync(int trainerId, int pokemonId);
        Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int trainerId);
    }
}
