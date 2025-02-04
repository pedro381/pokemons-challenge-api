using System.ComponentModel.DataAnnotations;

namespace PokemonsChallenge.Domain.Entities
{
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
