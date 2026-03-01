using Microsoft.AspNetCore.Mvc;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Models.DTOs;
using MySecureBackend.WebApi.Repositories;

namespace MySecureBackend.WebApi.Controllers
{
    [ApiController]
    [Route("api/objects")]
    public class Object2DController : ControllerBase
    {
        private readonly Object2DRepository _repository;
        private readonly EnvironmentRepository _environmentRepository;

        public Object2DController(
            Object2DRepository repository,
            EnvironmentRepository environmentRepository)
        {
            _repository = repository;
            _environmentRepository = environmentRepository;
        }

        // GET: api/objects/environment/1?userId=2
        [HttpGet("environment/{envId}")]
        public async Task<IActionResult> GetByEnvId(int envId, [FromQuery] int userId)
        {
            var env = await _environmentRepository.GetById(envId);
            if (env == null)
                return NotFound();

            if (env.UserId != userId)
                return Unauthorized("Not your environment.");

            var objects = await _repository.GetByEnvId(envId);
            return Ok(new { objects = objects });
        }

        // POST: api/objects
        [HttpPost]
        public async Task<IActionResult> Create(CreateObject2DRequest request)
        {
            var env = await _environmentRepository.GetById(request.EnvironmentId);
            if (env == null)
                return NotFound("Environment not found.");

            var obj = new Object2D
            {
                EnvironmentId = request.EnvironmentId,
                ObjectType = request.ObjectType,
                PosX = request.PosX,
                PosY = request.PosY,
                Rotation = request.Rotation,
                Scale = request.Scale
            };

            await _repository.Create(obj);
            return Ok("Object created.");
        }

        // DELETE: api/objects/5?userId=2
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int userId)
        {
            var obj = await _repository.GetByEnvId(id);
            if (obj == null)
                return NotFound();

            var env = await _environmentRepository.GetById(id);
            if (env.UserId != userId)
                return Unauthorized("Not your object.");

            await _repository.Delete(id);
            return Ok("Object deleted.");
        }

        // DELETE: api/objects/environment/1?userId=2
        [HttpDelete("environment/{envId}")]
        public async Task<IActionResult> DeleteByEnv(int envId, [FromQuery] int userId)
        {
            var env = await _environmentRepository.GetById(envId);
            if (env == null)
                return NotFound();

            if (env.UserId != userId)
                return Unauthorized("Not your environment.");

            await _repository.DeleteByEnv(envId);
            return Ok("Objects deleted.");
        }
    }
}