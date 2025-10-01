using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly SistemaDeTarefaDBContext _context;

        public UsuarioController(SistemaDeTarefaDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<UsuarioModel>>> GetUsuario()
        {
            return await _context.Usuarios.ToListAsync();        
        }

        [HttpPost]

        public async Task<ActionResult<UsuarioModel>> PostUsuario(UsuarioModel usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id, usuario });
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioModel>> PutUsuario(int id, UsuarioModel usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<UsuarioModel>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return BadRequest();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();


        }

    }
}
