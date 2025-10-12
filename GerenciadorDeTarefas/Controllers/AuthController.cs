using GerenciadorDeTarefas.DTOs;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repository.Interface;
using GerenciadorDeTarefas.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        
        public AuthController(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")] 
        public async Task<ActionResult<UsuarioResponseDTO>> Register([FromBody] UsuarioRegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            {
                return BadRequest("Nome, Email e Senha são obrigatórios para o registro.");
            }

            var usuarioExistente = (await _usuarioRepository.GetAllAsync()).FirstOrDefault(u => u.Email == dto.Email);
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

            await _usuarioRepository.PostAsync(novoUsuario);
            await _usuarioRepository.SaveChangesAsync();

            var responseDto = new UsuarioResponseDTO
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email
            };

            return CreatedAtAction(nameof(Register), responseDto);
        }

        [HttpPost("login")] 
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Senha))
            {
                return BadRequest("Nome e Senha são obrigatórios para o login.");
            }

            
            var usuario = (await _usuarioRepository.GetAllAsync()).FirstOrDefault(u => u.Nome == dto.Nome);

            if (usuario == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            
            if (!BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                return Unauthorized("Credenciais inválidas.");
            }

           var token = _tokenService.GenerateToken(usuario);

           var responseDto = new TokenDTO
            {
                Token = token,
                User = new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };

            return Ok(responseDto);
        }
    }
}