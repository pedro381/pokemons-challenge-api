namespace PokemonsChallenge.Domain.Entities
{
    public class CapturedPokemon
    {
        public int Id { get; set; }


        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }


        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }

        public DateTime CapturedAt { get; set; }
    }
}
