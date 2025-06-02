using System;
using ProjetoGerenciamento.Domain.Enums;

namespace ProjetoGerenciamento.Application.DTOs
{
    public class ProjetoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime PrevisaoTermino { get; set; }
        public DateTime? DataTerminoReal { get; set; }
        public decimal OrcamentoTotal { get; set; }
        public StatusProjeto Status { get; set; }
        public RiscoProjeto ClassificacaoRisco { get; set; }
        
    }
}