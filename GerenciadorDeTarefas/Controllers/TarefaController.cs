using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [Route("api/tarefas")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _repository;

        public TarefaController(ITarefaRepository repository)
        {
            _repository = repository;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaDTO>>> GetAllAsync()
        {
            var tarefas = await _repository.GetAllAsync();

           

            var dtos = tarefas.Select(t => new TarefaDTO
            {
                Id = t.Id,
                Titulo = t.Titulo,
                DataCriacao = t.DataCriacao,
                ProjetoId = t.ProjetoId,
                UsuarioId = t.UsuarioId,
                UsuarioNome = t.Usuario?.Nome,
                ProjetoNome = t.Projeto?.Nome,
                StatusTarefa = Enum.Parse<Status>(t.StatusTarefa.ToString()),
                PrioridadeTarefa = Enum.Parse<Prioridade>(t.PrioridadeTarefa.ToString()),
                Tags = t.Tags?.Select(tag => tag.Nome).ToList()
            });

            return Ok(dtos);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<TarefaDTO>> GetByIdAsync(int id, [FromQuery] int? usuarioId = null)
        {
            var tarefa = await _repository.GetByIdAsync(id);
            if (tarefa == null)
                return NotFound();

            if (usuarioId.HasValue && tarefa.UsuarioId != usuarioId.Value)
                return Forbid();

            if (!Enum.TryParse<Status>(tarefa.StatusTarefa.ToString(), true, out var status))
                return BadRequest("StatusTarefa inválido.");

            if (!Enum.TryParse<Prioridade>(tarefa.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa inválida.");

            return Ok(new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                DataCriacao = tarefa.DataCriacao,
                ProjetoId = tarefa.ProjetoId,
                UsuarioId = tarefa.UsuarioId,
                UsuarioNome = tarefa.Usuario?.Nome,
                ProjetoNome = tarefa.Projeto?.Nome,
                StatusTarefa = status,
                PrioridadeTarefa = prioridade,
                Tags = tarefa.Tags?.Select(tag => tag.Nome).ToList()
            });
        }

       
        [HttpPost]
        public async Task<ActionResult<TarefaDTO>> PostAsync(
        [FromBody] TarefaDTO dto,
        [FromServices] IProjetoRepository projetoRepository,
        [FromServices] ITagRepository tagRepository,
        [FromServices] IUsuarioRepository usuarioRepository) 
        {
            if (dto.UsuarioId <= 0)
                return BadRequest("UsuarioId deve ser informado e maior que zero.");

            if (!Enum.TryParse<Status>(dto.StatusTarefa.ToString(), true, out var status))
                return BadRequest("StatusTarefa inválido.");

            if (!Enum.TryParse<Prioridade>(dto.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa inválida.");

            
            var usuario = await usuarioRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
            {
                
                return NotFound("Usuário associado à tarefa não encontrado.");
            }

            
            var projeto = (await projetoRepository.GetAllAsync())
                .FirstOrDefault(p => string.Equals(p.Nome.Trim(), dto.ProjetoNome?.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && p.UsuarioId == dto.UsuarioId);

            if (projeto == null)
            {
                projeto = new ProjetoModel
                {
                    Nome = dto.ProjetoNome.Trim() ?? "Projeto Sem Nome",
                    UsuarioId = dto.UsuarioId,
                   
                    UsuarioNome = usuario.Nome
                };

                await projetoRepository.PostAsync(projeto);
                await projetoRepository.SaveChangesAsync();
                
            }

            var tags = new List<TagModel>();
            if (dto.Tags != null && dto.Tags.Any())
            {
                var todasTags = await tagRepository.GetAllAsync();
                foreach (var tagNome in dto.Tags)
                {
                    var tag = todasTags.FirstOrDefault(t =>
                        string.Equals(t.Nome.Trim(), tagNome.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (tag == null)
                    {
                        tag = new TagModel { Nome = tagNome.Trim() };
                        await tagRepository.PostAsync(tag);
                        await tagRepository.SaveChangesAsync();
                    }

                    tags.Add(tag);
                }
            }

            var tarefa = new TarefaModel
            {
                Titulo = dto.Titulo.Trim(),
                DataCriacao = dto.DataCriacao,
                StatusTarefa = status,
                PrioridadeTarefa = prioridade,
                ProjetoId = projeto.Id,
                UsuarioId = dto.UsuarioId,
                Tags = tags
            };

            await _repository.PostAsync(tarefa);
            await _repository.SaveChangesAsync();

            
            var dtoResult = new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                DataCriacao = tarefa.DataCriacao,
                ProjetoId = tarefa.ProjetoId,
                ProjetoNome = projeto.Nome,
                UsuarioId = tarefa.UsuarioId,
                UsuarioNome = usuario.Nome,
                StatusTarefa = status,
                PrioridadeTarefa = prioridade,
                Tags = tarefa.Tags.Select(t => t.Nome).ToList()
            };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = tarefa.Id }, dtoResult);
        }


        
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(
            int id,
            [FromBody] TarefaDTO dto,
            [FromServices] ITagRepository tagRepository,
            [FromServices] IProjetoRepository projetoRepository,
            [FromServices] IUsuarioRepository usuarioRepository) 
        {
            
            var usuario = await usuarioRepository.GetByIdAsync(dto.UsuarioId);
            if (usuario == null)
            {
                return NotFound("Usuário associado à tarefa não encontrado.");
            }

            var projeto = (await projetoRepository.GetAllAsync())
                .FirstOrDefault(p => p.Nome.ToLower() == dto.ProjetoNome.ToLower()
                                     && p.UsuarioId == dto.UsuarioId);

            if (projeto == null)
            {
                projeto = new ProjetoModel
                {
                    Nome = dto.ProjetoNome,
                    UsuarioId = dto.UsuarioId,
                   
                    UsuarioNome = usuario.Nome
                };
                await projetoRepository.PostAsync(projeto);
                await projetoRepository.SaveChangesAsync();
            }

            if (!Enum.TryParse<Status>(dto.StatusTarefa.ToString(), true, out var status))
                return BadRequest("StatusTarefa não pode ser nulo.");

            if (!Enum.TryParse<Prioridade>(dto.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa não pode ser nulo.");

            var tarefaExistente = await _repository.GetByIdAsync(id);
            if (tarefaExistente == null)
                return NotFound();

            tarefaExistente.Titulo = dto.Titulo;
            tarefaExistente.ProjetoId = projeto.Id;
            tarefaExistente.UsuarioId = dto.UsuarioId;
            tarefaExistente.StatusTarefa = status;
            tarefaExistente.PrioridadeTarefa = prioridade;

            
            if (dto.Tags != null && dto.Tags.Any())
            {
                var todasTags = await tagRepository.GetAllAsync();
                var tags = new List<TagModel>();
                foreach (var tagNome in dto.Tags)
                {
                    var tag = todasTags.FirstOrDefault(t =>
                        string.Equals(t.Nome, tagNome, StringComparison.OrdinalIgnoreCase));

                    if (tag == null)
                    {
                        tag = new TagModel { Nome = tagNome };
                        await tagRepository.PostAsync(tag);
                        await tagRepository.SaveChangesAsync();
                    }

                    tags.Add(tag);
                }

                tarefaExistente.Tags = tags;
            }
            else
            {
               
                if (tarefaExistente.Tags != null)
                {
                    tarefaExistente.Tags.Clear();
                }
            }

            await _repository.UpdateAsync(tarefaExistente);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var tarefa = await _repository.GetByIdAsync(id);
            if (tarefa == null)
                return NotFound();

            await _repository.DeleteAsync(tarefa);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}