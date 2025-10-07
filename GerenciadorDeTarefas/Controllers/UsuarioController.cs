using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuario()
        {
            var usuario = await _repository.GetAllAsync();
            var dtos = usuario.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email
            });
            return Ok(dtos);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioById(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);

            if (usuario == null) return NotFound();

            var dto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return Ok(dto);
        }

        
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario([FromBody] UsuarioDTO dto)
        {
           
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("Nome e Email são obrigatórios.");
            }

            var usuarioExistente = new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email
            };

            await _repository.PostAsync(usuarioExistente);
            await _repository.SaveChangesAsync();

           
            return CreatedAtAction(nameof(GetUsuarioById), new { id = usuarioExistente.Id }, new UsuarioDTO
            {
                Id = usuarioExistente.Id,
                Nome = usuarioExistente.Nome,
                Email = usuarioExistente.Email
            });
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUsuario(int id, [FromBody] UsuarioDTO dto)
        {
            var usuarioExistente = await _repository.GetByIdAsync(id);
            if (usuarioExistente == null) return NotFound();

            usuarioExistente.Nome = dto.Nome;
            usuarioExistente.Email = dto.Email;

            await _repository.UpdateAsync(usuarioExistente);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);

            if (usuario == null) return NotFound();

            await _repository.DeleteAsync(usuario);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
    }
}