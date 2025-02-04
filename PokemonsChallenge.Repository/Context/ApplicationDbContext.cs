using Microsoft.EntityFrameworkCore;
using PokemonsChallenge.Domain.Entities;

namespace PokemonsChallenge.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }  // Entidade Pokémon já existente
        public DbSet<CapturedPokemon> CapturedPokemons { get; set; }
    }
}
