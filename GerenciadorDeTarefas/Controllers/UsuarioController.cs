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

       
        [HttpPost]
        [Route("register")] 
        public async Task<ActionResult<UsuarioResponseDTO>> PostUsuario([FromBody] UsuarioRegisterDTO dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            {
                return BadRequest("Nome, Email e Senha são obrigatórios para o registro.");
            }

            var usuarioExistente = (await _repository.GetAllAsync()).FirstOrDefault(u => u.Email == dto.Email);
            if (usuarioExistente != null)
            {
                return Conflict("Este e-mail já está cadastrado.");
            }

           
            string senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var novoUsuario = new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = senhaHash 
            };

            await _repository.PostAsync(novoUsuario);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarioById), new { id = novoUsuario.Id }, new UsuarioResponseDTO
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email
            });
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