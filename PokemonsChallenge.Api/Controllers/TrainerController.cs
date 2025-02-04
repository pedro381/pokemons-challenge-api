using Microsoft.AspNetCore.Mvc;
using PokemonsChallenge.Domain.Entities;
using PokemonsChallenge.Service.Interfaces;
using System;

namespace PokemonsChallenge.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        // POST: api/Trainer
        [HttpPost]
        public async Task<IActionResult> RegisterTrainer([FromBody] Trainer trainer)
        {
            if (trainer == null)
            {
                return BadRequest(new { message = "Trainer data is required." });
            }

            try
            {
                var registeredTrainer = await _trainerService.RegisterTrainerAsync(trainer);
                if (registeredTrainer == null)
                {
                    return StatusCode(500, new { message = "A problem occurred while registering the trainer." });
                }

                return CreatedAtAction(nameof(GetTrainerById), new { id = registeredTrainer.Id }, registeredTrainer);
            }
            catch (Exception ex)
            {
                // Aqui podemos fazer log do erro, se necessário.
                return StatusCode(500, new { message = "An error occurred while registering the trainer.", details = ex.Message });
            }
        }

        // GET: api/Trainer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainerById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid trainer ID." });
            }

            try
            {
                var trainer = await _trainerService.GetTrainerByIdAsync(id);
                if (trainer == null)
                {
                    return NotFound(new { message = $"Trainer with ID {id} not found." });
                }

                return Ok(trainer);
            }
            catch (Exception ex)
            {
                // Aqui também podemos registrar o erro, se necessário.
                return StatusCode(500, new { message = "An error occurred while fetching the trainer.", details = ex.Message });
            }
        }

        // GET: api/Trainer/list
        [HttpGet("list")]
        public async Task<IActionResult> GetTrainerList()
        {

            try
            {
                var trainer = await _trainerService.GetTrainerListAsync();

                return Ok(trainer);
            }
            catch (Exception ex)
            {
                // Aqui também podemos registrar o erro, se necessário.
                return StatusCode(500, new { message = "An error occurred while fetching the trainer.", details = ex.Message });
            }
        }
    }
}
