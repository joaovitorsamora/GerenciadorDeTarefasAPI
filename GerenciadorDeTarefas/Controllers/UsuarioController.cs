using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 
using System.Security.Claims; 

namespace GerenciadorDeTarefas.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuario()
        {
            var usuarios = await _repository.GetAllAsync();
            var dtos = usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuarioById(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);

            if (usuario == null) return NotFound();

            var dto = new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return Ok(dto);
        }


        [Authorize] 
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUsuario(int id, [FromBody] UsuarioRegisterDTO dto)
        {
           
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

          
            if (id != loggedInUserId)
            {
                return Forbid("Você só pode atualizar seu próprio perfil.");
            }

            var usuarioExistente = await _repository.GetByIdAsync(id);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nome = dto.Nome ?? usuarioExistente.Nome;
            usuarioExistente.Email = dto.Email ?? usuarioExistente.Email;

           
            if (!string.IsNullOrWhiteSpace(dto.Senha))
            {
                usuarioExistente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            }

            await _repository.UpdateAsync(usuarioExistente);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        
        [Authorize] 
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
           
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

          
            if (id != loggedInUserId)
            {
                return Forbid("Você só pode excluir seu próprio perfil.");
            }

            var usuario = await _repository.GetByIdAsync(id);

            if (usuario == null) return NotFound();

            await _repository.DeleteAsync(usuario);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}