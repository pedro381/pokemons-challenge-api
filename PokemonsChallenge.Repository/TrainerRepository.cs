using Microsoft.EntityFrameworkCore;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository.Interfaces;

namespace PokemonsChallenge.Repository
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Trainer> AddTrainerAsync(Trainer trainer)
        {
            try
            {
                if (trainer == null)
                {
                    throw new ArgumentNullException(nameof(trainer), "Trainer cannot be null");
                }

                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                return trainer;
            }
            catch (ArgumentNullException ex)
            {
                // Lança exceção caso o trainer seja nulo
                throw new ArgumentException("Trainer cannot be null", ex);
            }
            catch (DbUpdateException ex)
            {
                // Exceção ao tentar salvar no banco de dados
                throw new Exception("An error occurred while saving the trainer", ex);
            }
            catch (Exception ex)
            {
                // Captura qualquer outra exceção genérica
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task<List<Trainer>> GetTrainerListAsync()
        {
            try
            {
                var trainer = await _context.Trainers.ToListAsync();
                return trainer;
            }
            catch (KeyNotFoundException ex)
            {
                // Lança exceção caso o treinador não seja encontrado
                throw new Exception($"Trainer not found", ex);
            }
            catch (Exception ex)
            {
                // Captura qualquer outra exceção genérica
                throw new Exception("An unexpected error occurred", ex);
            }
        }

        public async Task<Trainer> GetTrainerByIdAsync(int id)
        {
            try
            {
                var trainer = await _context.Trainers.FindAsync(id);
                if (trainer == null)
                {
                    throw new KeyNotFoundException($"Trainer with ID {id} not found");
                }

                return trainer;
            }
            catch (KeyNotFoundException ex)
            {
                // Lança exceção caso o treinador não seja encontrado
                throw new Exception($"Trainer with ID {id} not found", ex);
            }
            catch (Exception ex)
            {
                // Captura qualquer outra exceção genérica
                throw new Exception("An unexpected error occurred", ex);
            }
        }
    }
}
