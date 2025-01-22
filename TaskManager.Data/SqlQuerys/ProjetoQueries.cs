using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.SqlQuerys
{
    public static class ProjetoQueries
    {
        public const string GetAllProject =
            @" SELECT 
                    p.Id AS ProjetoId,
                    p.Nome AS ProjetoNome,
                    u.Nome AS UsuarioNome,
                    t.Id AS TarefaId,
                    t.Titulo AS TarefaTitulo,
                    t.Descricao AS TarefaDescricao,
                    t.DataVencimento AS TarefaDataVencimento,
                    t.Status AS TarefaStatus,
                    t.Prioridade AS TarefaPrioridade,
                    c.Id AS ComentarioId,
                    c.Texto AS ComentarioTexto,
                    c.DataComentario AS ComentarioData,
                    h.Id AS HistoricoId,
                    h.CampoModificado AS HistoricoCampoModificado,
                    h.ValorAntigo AS HistoricoValorAntigo,
                    h.ValorNovo AS HistoricoValorNovo,
                    h.DataModificacao AS HistoricoDataModificacao
                FROM 
                    Projeto p
                JOIN 
                    Usuario u ON p.UsuarioId = u.Id
                LEFT JOIN 
                    Tarefa t ON t.ProjetoId = p.Id
                LEFT JOIN 
                    Comentario c ON c.TarefaId = t.Id
                LEFT JOIN 
                    HistoricoTarefa h ON h.TarefaId = t.Id
                ORDER BY 
                    p.Id, t.Id, c.Id, h.Id;
            ";        
    }
}
