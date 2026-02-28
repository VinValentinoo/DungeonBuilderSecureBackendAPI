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
        
        public Object2DController(Object2DRepository repository)
        {
            _repository = repository;
        }

        // GET: api/objects/environment/1
        [HttpGet("environment/{envId}")]
        public async Task<IActionResult> GetByEnvId(int envId)
        {
            var objects = await _repository.GetByEnvId(envId);
            return Ok(new { objects = objects });
        }

        // POST: api/objects
        [HttpPost]
        public async Task<IActionResult> Create(CreateObject2DRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ObjectType))
                return BadRequest("ObjectType is required.");

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

        // DELETE: api/objects/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return Ok("Object deleted.");
        }

        // DELTE: api/objects/environment/1
        [HttpDelete("environment/{envId}")]
        public async Task<IActionResult> DeleteByEnv(int envId)
        {
            await _repository.DeleteByEnv(envId);
            return Ok("Objects deleted.");
        }
    }
}
