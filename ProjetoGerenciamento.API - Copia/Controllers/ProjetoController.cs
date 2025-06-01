using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoGerenciamento.Application.DTOs;
using ProjetoGerenciamento.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoGerenciamento.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProjetoController(IProjetoService projetoService) : ControllerBase
    {
        private readonly IProjetoService _projetoService = projetoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoDto>>> Get()
        {
            var projetos = await _projetoService.BuscarTodosAsync();
            return Ok(projetos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetoDto>> Get(int id)
        {
            var projeto = await _projetoService.ObterPorIdAsync(id);
            if (projeto == null) return NotFound();
            return Ok(projeto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProjetoDto projetoDto)
        {
            var result = await _projetoService.InserirAsync(projetoDto);
            if (!result) return BadRequest("Erro ao inserir projeto");
            return CreatedAtAction(nameof(Get), new { id = projetoDto.Id }, projetoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProjetoDto projetoDto)
        {
            if (id != projetoDto.Id) return BadRequest("Id inválido");

            var result = await _projetoService.AtualizarAsync(projetoDto);
            if (!result) return BadRequest("Erro ao atualizar projeto");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _projetoService.ExcluirAsync(id);
            if (!result) return BadRequest("Não é possível excluir o projeto ou projeto não encontrado");
            return NoContent();
        }
    }
}
