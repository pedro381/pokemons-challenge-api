using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;

namespace PokemonsChallenge.Repository.Interfaces
{
    public interface IPokemonRepository
    {
        Task CapturePokemonAsync(int trainerId, PokemonDto pokemon);
        Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int trainerId);
    }
}
