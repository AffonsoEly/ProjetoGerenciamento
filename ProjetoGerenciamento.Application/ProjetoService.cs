using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetoGerenciamento.Application.DTOs;
using ProjetoGerenciamento.Application.Interfaces;
using ProjetoGerenciamento.Domain.Entities;
using ProjetoGerenciamento.Domain.Interfaces;
using AutoMapper; // Você pode usar AutoMapper para mapear entre Entity e DTO

namespace ProjetoGerenciamento.Application.Services
{
	public class ProjetoService : IProjetoService
	{
		private readonly IProjetoRepository _projetoRepository;
		private readonly IMapper _mapper;

		public ProjetoService(IProjetoRepository projetoRepository, IMapper mapper)
		{
			_projetoRepository = projetoRepository;
			_mapper = mapper;
		}

		public async Task<bool> InserirAsync(ProjetoDto projetoDto)
		{
			var projeto = _mapper.Map<Projeto>(projetoDto);
			// Aqui você pode colocar regras de negócio antes de inserir
			return await _projetoRepository.InserirAsync(projeto);
		}

		public async Task<bool> AtualizarAsync(ProjetoDto projetoDto)
		{
			var projeto = _mapper.Map<Projeto>(projetoDto);
			return await _projetoRepository.AtualizarAsync(projeto);
		}

		public async Task<bool> ExcluirAsync(int id)
		{
			// Regra: não excluir se status proibido (Iniciado, Planejado, Em andamento, Encerrado)
			var projeto = await _projetoRepository.ObterPorIdAsync(id);
			if (projeto == null) return false;

			if (projeto.Status == Domain.Enums.StatusProjeto.Iniciado ||
				projeto.Status == Domain.Enums.StatusProjeto.Planejado ||
				projeto.Status == Domain.Enums.StatusProjeto.EmAndamento ||
				projeto.Status == Domain.Enums.StatusProjeto.Encerrado)
			{
				return false; // Não pode excluir
			}

			return await _projetoRepository.ExcluirAsync(id);
		}

		public async Task<ProjetoDto> ObterPorIdAsync(int id)
		{
			var projeto = await _projetoRepository.ObterPorIdAsync(id);
			return _mapper.Map<ProjetoDto>(projeto);
		}

		public async Task<IEnumerable<ProjetoDto>> BuscarTodosAsync()
		{
			var projetos = await _projetoRepository.BuscarTodosAsync();
			return _mapper.Map<IEnumerable<ProjetoDto>>(projetos);
		}

		public async Task<IEnumerable<ProjetoDto>> BuscarPorNomeOuStatusAsync(string nome, string status)
		{
			var projetos = await _projetoRepository.BuscarPorNomeOuStatusAsync(nome, status);
			return _mapper.Map<IEnumerable<ProjetoDto>>(projetos);
		}
	}
}
