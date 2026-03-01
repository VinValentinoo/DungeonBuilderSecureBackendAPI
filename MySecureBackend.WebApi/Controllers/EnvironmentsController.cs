using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Models.DTOs;
using MySecureBackend.WebApi.Repositories;

namespace MySecureBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/environments")]
    public class EnvironmentsController : ControllerBase
    {
        private readonly EnvironmentRepository _environmentRepository;

        public EnvironmentsController(EnvironmentRepository environmentRepository)
        {
            _environmentRepository = environmentRepository;
        }

        // GET: api/environments/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var environments = await _environmentRepository.GetByUserId(userId);
            return Ok(environments);
        }

        // GET: api/environments/5?userId=1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int userId)
        {
            var environment = await _environmentRepository.GetById(id);
            if (environment == null)
                return NotFound();

            if (environment.UserId != userId)
                return Unauthorized("This environment does not belong to you.");

            return Ok(environment);
        }

        // POST: api/environments
        [HttpPost]
        public async Task<IActionResult> Create(CreateEnvironmentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name required.");

            if (request.Name.Length > 25)
                return BadRequest("Name too long.");

            if (request.Width < 20 || request.Width > 200)
                return BadRequest("Width must be between 20 and 200.");

            if (request.Height < 10 || request.Height > 100)
                return BadRequest("Height must be between 10 and 100.");

            if (await _environmentRepository.DoesNameExists(request.UserId, request.Name))
                return BadRequest("Environment name already exists.");

            if (await _environmentRepository.CountByUser(request.UserId) >= 5)
                return BadRequest("Max 5 environments allowed.");

            var env = new Environment2D
            {
                UserId = request.UserId,
                Name = request.Name,
                Width = request.Width,
                Height = request.Height
            };

            await _environmentRepository.Create(env);

            return Ok("Environment created.");
        }

        // DELETE: api/environments/5?userId=1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var env = await _environmentRepository.GetById(id);
            if (env == null)
                return NotFound();

            if (env.UserId != userId)
                return Unauthorized("This environment does not belong to you.");

            await _environmentRepository.Delete(id);
            return Ok("Environment deleted.");
        }
    }
}