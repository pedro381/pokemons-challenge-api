using PokemonsChallenge.Domain.Entities;

namespace PokemonsChallenge.Repository.Interfaces
{
    public interface ITrainerRepository
    {
        Task<Trainer> AddTrainerAsync(Trainer trainer);
        Task<Trainer> GetTrainerByIdAsync(int id);
        Task<List<Trainer>> GetTrainerListAsync();
    }
}
