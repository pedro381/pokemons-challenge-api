using Microsoft.EntityFrameworkCore;
using PokemonsChallenge.Domain.Dtos;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Repository.Interfaces;

namespace PokemonsChallenge.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ApplicationDbContext _context;

        public PokemonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CapturePokemonAsync(int trainerId, PokemonDto pokemonDto)
        {
            try
            {
                var trainer = await _context.Trainers.FindAsync(trainerId);
                if (trainer == null)
                {
                    throw new ArgumentException("Trainer not found.", nameof(trainerId));
                }

                var pokemon = await _context.Pokemons.FindAsync(pokemonDto.Id);
                if (pokemon == null)
                {
                    pokemon = new Pokemon
                    {
                        Id = pokemonDto.Id,
                        Name = pokemonDto.Name,
                        ImageUrl = pokemonDto.PokemonImageDto.Url,
                    };
                    _context.Pokemons.Add(pokemon);

                    await _context.SaveChangesAsync();
                }

                var capturedPokemon = new CapturedPokemon
                {
                    TrainerId = trainerId,
                    PokemonId = pokemonDto.Id,
                    CapturedAt = DateTime.UtcNow
                };

                _context.CapturedPokemons.Add(capturedPokemon);

                await _context.SaveChangesAsync();
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Error capturing Pokemon: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database error occurred while capturing the Pokémon.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while capturing the Pokémon.", ex);
            }
        }

        public async Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int trainerId)
        {
            try
            {
                var trainerExists = await _context.Trainers.AnyAsync(t => t.Id == trainerId);
                if (!trainerExists)
                {
                    throw new ArgumentException("Trainer not found.", nameof(trainerId));
                }

                var capturedPokemons = await _context.CapturedPokemons
                    .Where(cp => cp.TrainerId == trainerId)
                    .Include(cp => cp.Pokemon)
                    .ToListAsync();

                return capturedPokemons;
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Error retrieving captured Pokémon: {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Error retrieving captured Pokémon: {ex.Message}", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database error occurred while retrieving captured Pokémon.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving captured Pokémon.", ex);
            }
        }
    }
}
