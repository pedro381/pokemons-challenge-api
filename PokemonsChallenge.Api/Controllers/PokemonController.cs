using Microsoft.AspNetCore.Mvc;
using PokemonsChallenge.Api.DTOs;
using PokemonsChallenge.Service.Interfaces;
using System;

namespace PokemonsChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPokemons()
        {
            try
            {
                var pokemons = await _pokemonService.GetRandomPokemonsAsync();
                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                // Log da exceção (se necessário)
                return StatusCode(500, new { message = "An error occurred while fetching random pokemons.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid Pokemon ID." });
            }

            try
            {
                var pokemon = await _pokemonService.GetPokemonByIdAsync(id);
                if (pokemon == null)
                {
                    return NotFound(new { message = $"Pokemon with ID {id} not found." });
                }
                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                // Log da exceção (se necessário)
                return StatusCode(500, new { message = "An error occurred while fetching the pokemon.", details = ex.Message });
            }
        }

        [HttpPost("capture")]
        public async Task<IActionResult> CapturePokemon([FromBody] CapturePokemonDto capturePokemonDto)
        {
            if (capturePokemonDto == null || capturePokemonDto.TrainerId <= 0 || capturePokemonDto.PokemonId <= 0)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            try
            {
                await _pokemonService.CapturePokemonAsync(capturePokemonDto.TrainerId, capturePokemonDto.PokemonId);
                return Ok(new { message = "Pokemon captured successfully!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log da exceção (se necessário)
                return StatusCode(500, new { message = "An error occurred while capturing the pokemon.", details = ex.Message });
            }
        }

        [HttpGet("{trainerId}/captured")]
        public async Task<IActionResult> GetCapturedPokemons(int trainerId)
        {
            if (trainerId <= 0)
            {
                return BadRequest(new { message = "Invalid Trainer ID." });
            }

            try
            {
                var capturedPokemons = await _pokemonService.GetCapturedPokemonsAsync(trainerId);
                return Ok(capturedPokemons);
            }
            catch (Exception ex)
            {
                // Log da exceção (se necessário)
                return StatusCode(500, new { message = "An error occurred while fetching captured pokemons.", details = ex.Message });
            }
        }
    }
}
