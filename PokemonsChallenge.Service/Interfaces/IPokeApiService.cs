using Refit;
using PokemonsChallenge.Domain.Dtos;

namespace PokemonsChallenge.Service.Interfaces
{
    public interface IPokeApiService
    {
        [Get("/pokemon/{id}")]
        Task<PokemonDto> GetPokemonByIdAsync(int id);

        [Get("/pokemon?limit=10")]
        Task<PokemonListDto> GetRandomPokemonsAsync();
    }
}
