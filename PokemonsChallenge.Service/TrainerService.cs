using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository.Interfaces;
using PokemonsChallenge.Service.Interfaces;

namespace PokemonsChallenge.Service
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<Trainer> RegisterTrainerAsync(Trainer trainer)
        {
            if (trainer == null)
                throw new ArgumentNullException(nameof(trainer), "Trainer cannot be null.");

            try
            {
                return await _trainerRepository.AddTrainerAsync(trainer);
            }
            catch (Exception ex)
            {
                // Aqui, podemos capturar qualquer erro inesperado e lançar uma exceção mais específica
                throw new InvalidOperationException("An error occurred while registering the trainer.", ex);
            }
        }

        public async Task<Trainer> GetTrainerByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Trainer ID must be a positive integer.", nameof(id));

            try
            {
                var trainer = await _trainerRepository.GetTrainerByIdAsync(id);
                if (trainer == null)
                    throw new InvalidOperationException($"Trainer with ID {id} not found.");

                return trainer;
            }
            catch (Exception ex)
            {
                // Tratamento de erro para falhas inesperadas
                throw new InvalidOperationException("An error occurred while retrieving the trainer.", ex);
            }
        }

        public async Task<List<Trainer>> GetTrainerListAsync()
        {
            try
            {
                var trainer = await _trainerRepository.GetTrainerListAsync();
                return trainer;
            }
            catch (Exception ex)
            {
                // Tratamento de erro para falhas inesperadas
                throw new InvalidOperationException("An error occurred while retrieving the trainer.", ex);
            }
        }
    }
}
