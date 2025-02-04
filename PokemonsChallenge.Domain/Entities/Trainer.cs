using System.ComponentModel.DataAnnotations;

namespace PokemonsChallenge.Domain.Entities
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Cpf { get; set; }
    }
}
