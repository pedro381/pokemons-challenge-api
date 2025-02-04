using PokemonsChallenge.Domain.Entities;

namespace PokemonsChallenge.Service.Interfaces
{
    public interface ITrainerService
    {
        Task<Trainer> RegisterTrainerAsync(Trainer trainer);
        Task<Trainer> GetTrainerByIdAsync(int id);
        Task<List<Trainer>> GetTrainerListAsync();
    }
}
