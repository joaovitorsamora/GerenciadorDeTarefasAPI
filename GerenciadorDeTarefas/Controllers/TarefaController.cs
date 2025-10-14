using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                ProjetoNome = t.Projeto.Nome,
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

            if (!Enum.TryParse<Prioridade>(tarefa.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa inválida.");

            return Ok(new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                DataCriacao = tarefa.DataCriacao,
                ProjetoId = tarefa.ProjetoId,
                UsuarioId = tarefa.UsuarioId,
                ProjetoNome = tarefa.Projeto.Nome,
                PrioridadeTarefa = prioridade,
                Tags = tarefa.Tags?.Select(tag => tag.Nome).ToList()
            });
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TarefaDTO>> PostAsync(
    [FromBody] TarefaDTO dto,
    [FromServices] IProjetoRepository projetoRepository,
    [FromServices] ITagRepository tagRepository,
    [FromServices] IUsuarioRepository usuarioRepository)
        {

            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var loggedInUserId))
                return Unauthorized("Usuário não autenticado ou token inválido.");

            int usuarioId = loggedInUserId;


            if (!Enum.TryParse<Status>(dto.StatusTarefa.ToString(), true, out var status))
                return BadRequest("StatusTarefa inválido.");

            if (!Enum.TryParse<Prioridade>(dto.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa inválida.");

            // 3. Gerencia o projeto
            ProjetoModel projeto = null;
            if (!string.IsNullOrWhiteSpace(dto.ProjetoNome))
            {
                projeto = (await projetoRepository.GetAllAsync())
                    .FirstOrDefault(p => p.UsuarioId == usuarioId &&
                                         p.Nome.Trim().Equals(dto.ProjetoNome.Trim(), StringComparison.OrdinalIgnoreCase));

                if (projeto == null)
                {
                    var usuario = await usuarioRepository.GetByIdAsync(usuarioId);
                    if (usuario == null) return NotFound("Usuário logado não encontrado.");

                    projeto = new ProjetoModel
                    {
                        Nome = dto.ProjetoNome.Trim(),
                        UsuarioId = usuarioId,
                        UsuarioNome = usuario.Nome
                    };

                    await projetoRepository.PostAsync(projeto);
                    await projetoRepository.SaveChangesAsync();
                }
            }

            var tagsExistentes = await tagRepository.GetAllAsync();
            var tagsFinais = new List<TagModel>();

            if (dto.Tags != null && dto.Tags.Any())
            {
                foreach (var tagNome in dto.Tags)
                {
                    var tag = tagsExistentes.FirstOrDefault(t =>
                        t.Nome.Trim().Equals(tagNome.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (tag == null)
                    {
                        tag = new TagModel { Nome = tagNome.Trim() };
                        await tagRepository.PostAsync(tag);
                        await tagRepository.SaveChangesAsync();
                    }

                    tagsFinais.Add(tag);
                }
            }


            var tarefa = new TarefaModel
            {
                Titulo = dto.Titulo.Trim(),
                DataCriacao = dto.DataCriacao,
                StatusTarefa = status,
                PrioridadeTarefa = prioridade,
                ProjetoId = projeto.Id,
                UsuarioId = usuarioId,
                Tags = tagsFinais
            };

            await _repository.PostAsync(tarefa);
            await _repository.SaveChangesAsync();

            var dtoResult = new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                DataCriacao = tarefa.DataCriacao,
                ProjetoId = projeto.Id,
                UsuarioId = tarefa.UsuarioId,
                ProjetoNome = projeto.Nome,
                PrioridadeTarefa = prioridade,
                StatusTarefa = status,
                Tags = tagsFinais.Select(t => t.Nome).ToList()
            };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = tarefa.Id, usuarioId }, dtoResult);
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

            if (!Enum.TryParse<Prioridade>(dto.PrioridadeTarefa.ToString(), true, out var prioridade))
                return BadRequest("PrioridadeTarefa não pode ser nulo.");

            var tarefaExistente = await _repository.GetByIdAsync(id);
            if (tarefaExistente == null)
                return NotFound();

            tarefaExistente.Titulo = dto.Titulo;
            tarefaExistente.ProjetoId = projeto.Id;
            tarefaExistente.UsuarioId = dto.UsuarioId;
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