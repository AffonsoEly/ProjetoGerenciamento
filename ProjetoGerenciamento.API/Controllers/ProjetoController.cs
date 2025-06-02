using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoGerenciamento.Application.DTOs;
using ProjetoGerenciamento.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoGerenciamento.API.Controllers
{
    [Authorize] // Todos endpoints requerem autenticação
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly IProjetoService _projetoService;

        public ProjetoController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoDto>>> Get()
        {
            var projetos = await _projetoService.BuscarTodosAsync();
            return Ok(projetos);
        }

        [Authorize(Roles = "Admin")] // Apenas Admin pode criar projetos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProjetoDto projetoDto)
        {
            var result = await _projetoService.InserirAsync(projetoDto);
            if (!result) return BadRequest("Erro ao inserir projeto");
            return CreatedAtAction(nameof(Get), new { id = projetoDto.Id }, projetoDto);
        }

        [Authorize(Roles = "Admin")] // Apenas Admin pode atualizar
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProjetoDto projetoDto)
        {
            if (id != projetoDto.Id) return BadRequest("Id inválido");

            var result = await _projetoService.AtualizarAsync(projetoDto);
            if (!result) return BadRequest("Erro ao atualizar projeto");

            return NoContent();
        }

        [Authorize(Roles = "Admin")] // Apenas Admin pode deletar
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _projetoService.ExcluirAsync(id);
            if (!result) return BadRequest("Não é possível excluir o projeto ou projeto não encontrado");
            return NoContent();
        }
        [Authorize(Roles = "Admin")] // Apenas Admin pode criar projetos
        [HttpPost("novo")]
        public async Task<ActionResult> NovoProjeto([FromBody] ProjetoDto projetoDto)
        {
            var result = await _projetoService.InserirAsync(projetoDto);
            if (!result)
                return BadRequest("Erro ao inserir projeto");

            return CreatedAtAction(nameof(Get), new { id = projetoDto.Id }, projetoDto);
        }
    }
}
